using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml.Linq;
using Common.Collections;
using Common.Res;

namespace Common.Wpf.Res
{
    [MarkupExtensionReturnType(typeof(object))]
    [ContentProperty("Parameters")]
    public class ResExtension : ManagedMarkupExtension, IServiceProvider
    {
        private readonly ResParamList parameters = new ResParamList();
        /// <summary>
        /// Collection of <see cref="ResKeyPart"/>s and <see cref="ResParam"/>s.
        /// In one collection for shortening in XAML.
        /// It's supposed there rarely will be mix of <see cref="ResKeyPart"/> and <see cref="ResParam"/>
        /// so we can put it to one collection and use reduced XAML
        /// </summary>
        public ResParamList Parameters
        {
            get { return parameters; }
            set
            {
                parameters.Clear();
                foreach (ResParamBase item in value.OfType<ResParamBase>())
                {
                    parameters.Add(item);
                }
            }
        }

        private WeakReference innerElement;
        private object innerProperty;

        public ResExtension()
        {
        }

        public ResExtension(string key)
            : this()
        {
            Key = key;
        }

        public ResExtension(string key, string defaultValue)
            : this(key)
        {
            if (!string.IsNullOrEmpty(defaultValue))
            {
                DefaultValue = defaultValue;
            }
        }

        protected virtual IResKeyProvider CreateDefaultKeyProvider()
        {
            return new ResKeyProvider(key);
        }
        
        private IResKeyProvider keyProvider;
        public IResKeyProvider KeyProvider
        {
            get { return keyProvider ?? CreateDefaultKeyProvider(); }
            set { keyProvider = value; }
        }

        private string key;
        public string Key
        {
            get { return KeyProvider.ProvideKey(null); }
            set { key = value; }
        }

        public string DefaultValue { get; set; }

        /// <summary>
        /// Returns default value for <paramref name="key"/>.
        /// This method is called when no resources was found for <paramref name="key"/>
        /// For "Some_Key" returns "#Some_Key"
        /// </summary>
        public object GetDefaultValue(string key)
        {
            object result = DefaultValue;
            if (TargetProperty == null) return result;
            Type targetType = TargetPropertyType;
            if (DefaultValue == null)
            {
                if (targetType == typeof(String) || targetType == typeof(object))
                {
                    result = String.Format("#{0}", key);
                }
            }
            else
            {
                if (targetType != typeof(String) && targetType != typeof(object))
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(targetType);
                    result = tc.ConvertFromInvariantString(DefaultValue);
                }
            }
            return result;
        }

        private void EnsureAppResourcesLoaded()
        {
            Assembly resourceDescriptionAssembly = Assembly.GetExecutingAssembly();
            //Assembly assembly = Assembly.Load(new AssemblyName(asmName));
            Stream resourcesReferencesStream = resourceDescriptionAssembly.GetManifestResourceStream("VI.Common.Wpf.Res.resources.xml");

            if (resourcesReferencesStream == null)
                return;

            XElement rootItem;
            using (TextReader streamReader = new StreamReader(resourcesReferencesStream))
            {
                XDocument doc = XDocument.Load(streamReader);
                rootItem = doc.Nodes().OfType<XElement>().FirstOrDefault();
            }

            if (rootItem == null) return;

            foreach (XElement resourceElement in rootItem.Elements())
            {
                try
                {
                    string asmName = resourceElement.Attribute("AssemblyName").Value;
                    string typeName = resourceElement.Attribute("ResourceRegistratorName").Value;
                    string methodName = resourceElement.Attribute("ResourceRegistratorMethodName").Value;

                    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains(asmName));

                    //Assembly assembly = Assembly.Load(new AssemblyName(asmName));

                    if (assembly != null)
                    {
                        Type resourceRegistrator = assembly.GetType(typeName);
                        MethodInfo method = resourceRegistrator.GetMethod(methodName);
                        method.Invoke(null, null);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("error = {0}", ex.Message));
                }
            }
        }

        private ResKeyPart keyPart1;
        /// <summary>
        /// Synthetic property that makes XAML more short and simple for common cases,
        /// when Key has only one key part
        /// Binding from this property will be set as first parameters in <see cref="Parameters"/> property
        /// </summary>
        public ResKeyPart KeyPart1
        {
            get { return keyPart1; }
            set
            {
                if (keyPart1 == value) return;
                if (value != null && Parameters.Count > 0 && Parameters[0] == value)
                    Parameters.RemoveAt(0);
                keyPart1 = value;
                Parameters.Insert(0, keyPart1);
            }
        }

        /// <summary>
        /// Our goal dynamically react to current culture and parameters changings.
        /// So we can't just return localized object. We create binding which 
        /// via ProvideValue() returns BindingExpression.
        /// This BindingExpression provides dynamic refresh when current culture or parameters are changing.
        /// In spite of it's name the method returns BindingExpression, not localized value
        /// </summary>
        protected override object GetValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key)) throw new ArgumentException("Key cannot be null");

            IProvideValueTarget targetHelper = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (!(targetHelper.TargetProperty is DependencyProperty))
            {
                // It's setter. Return this for calling this method on real target object
                return this;
            }
            DependencyObject targetObject = targetHelper.TargetObject as DependencyObject;
            if (targetObject != null && DesignerProperties.GetIsInDesignMode(targetObject))
                EnsureAppResourcesLoaded();

            ResParamList paramList = null;
            if (targetObject != null && Parameters.Count == 0)
                paramList = GetResContext(targetObject);
            if (paramList == null)
                paramList = Parameters;

            var converter = new ResConverter { KeyProvider = KeyProvider, ResExtension = this, Parameters = paramList };

            Binding binding = new Binding("UICulture")
            {
                Source = CultureManager.Instance,
                Mode = BindingMode.OneWay
            };
            // For simple key just watch for culture changing
            if (paramList.Count == 0)
            {
                binding.Converter = converter;
                return binding.ProvideValue(serviceProvider);
            }

            // For composite key or formatted string watch for culture, key parts or parameters changing
            var multiBinding = new MultiBinding { Mode = BindingMode.OneWay, Converter = converter };

            foreach (var param in paramList)
            {
                multiBinding.Bindings.Add(param);
            }
            // Converter waits the culter as last parameter
            multiBinding.Bindings.Add(binding);

            return multiBinding.ProvideValue(serviceProvider);
        }

        public static void BindExtension(DependencyObject element, DependencyProperty property, string key)
        {
            BindExtension(element, property, key, null);
        }

        public static void BindExtension(DependencyObject element, DependencyProperty property, string key, IEnumerable<ResParamBase> resParams)
        {
            if (element == null || property == null || key == null) return;
            var res = new ResExtension(key, string.Empty)
            {
                innerElement = new WeakReference(element),
                innerProperty = property
            };
            if (resParams != null)
                res.Parameters.AddRange(resParams);
            element.SetValue(property, res.ProvideValue(res));
        }

        /// <summary>
        /// Returns localized string taking parameters from object properties
        /// </summary>
        /// <param name="source">Object which property values will be taken as parameters for formatted string</param>
        /// <param name="key">Resource key</param>
        /// <param name="formatPropertyNames">From these properties method will take values and put them to formatted string </param>
        /// <returns>Localized formatted string</returns>
        public static string GetFormattedStringValue(object source, string key, IEnumerable<string> formatPropertyNames)
        {
            var valueFreezable = new ValueFreezable();
            BindExtension(valueFreezable, ValueFreezable.ValueProperty, key,
                formatPropertyNames.Select(s => new ResParam(s) { Source = source, Mode = BindingMode.OneWay }));
            return valueFreezable.Value as string;
        }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(IProvideValueTarget) && innerElement.Target != null && innerElement.IsAlive
                ? new ProvideValueTarget(innerElement.Target, innerProperty)
                : null;
        }

        public class ProvideValueTarget : IProvideValueTarget
        {
            public ProvideValueTarget(object element, object property)
            {
                TargetObject = element;
                TargetProperty = property;
            }

            public object TargetObject { get; private set; }
            public object TargetProperty { get; private set; }
        }

        public static readonly DependencyProperty ResContextProperty =
            DependencyProperty.RegisterAttached(
                "ResContext",
                typeof(ResParamList),
                typeof(ResExtension),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static ResParamList GetResContext(DependencyObject obj)
        {
            return (ResParamList)obj.GetValue(ResContextProperty);
        }

        public static void SetResContext(DependencyObject obj, ResParamList value)
        {
            obj.SetValue(ResContextProperty, value);
        }
    }
}