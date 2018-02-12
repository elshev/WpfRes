using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Common.Res;

namespace Common.Wpf.Res
{
    /// <summary>
    /// Converts resource key to localized string (object)
    /// Used as helper class for <see cref="ResExtension"/>
    /// </summary>
    internal class ResConverter : IValueConverter, IMultiValueConverter
    {
        /// <summary> This property set from outside by <see cref="ResExtension"/> </summary>
        public IResKeyProvider KeyProvider { get; set; }

        /// <summary>
        /// <see cref="ResExtension"/> set itself here.
        /// </summary>
        public ResExtension ResExtension { get; set; }

        /// <summary>
        /// Parameters for <see cref="ResExtension"/>
        /// </summary>
        public ResParamList Parameters { get; set; }

        /// <summary>
        /// <paramref name="value"/> has to contain value of <see cref="CultureInfo"/> type
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string key = KeyProvider.ProvideKey(new[] { value });
            var cultureInfo = value as CultureInfo;
            object localizedObject = ResManager.Instance.GetResourceObject(key, cultureInfo);
            return localizedObject ?? ResExtension.GetDefaultValue(key);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        #region IMultiValueConverter Members

        /// <summary>
        /// Returns localized string (object) by composite key.
        /// Key parts are in <paramref name="values"/> except last member of array.
        /// Last member is <see cref="CultureInfo"/>
        /// Firstly <see cref="KeyProvider"/> makes key from it's BaseKey and key parts from <paramref name="values"/>.
        /// Then localized object returned from resources depending on this key
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Parameters.Count + 1 != values.Length)
                throw new ArgumentException(string.Format("ResConverter - parameters count error. Parameters.Count = {0}, values.Count = {1}", Parameters.Count, values.Length));
            // values are the mix from ResKeyParts and ResParams. And culture in last parameter.
            // Separate them as Cinderella to different heaps.
            var resKeyParts = new List<object>();
            var resParams = new List<object>();
            for (int i = 0; i < Parameters.Count; i++)
            {
                object value = values[i];
                BindingBase param = Parameters[i];
                if (param is ResKeyPart)
                {
                    if (value == DependencyProperty.UnsetValue && param.FallbackValue != null)
                        value = param.FallbackValue;
                    if (value != null)
                        resKeyParts.Add(value);
                }
                else if (param is ResParam)
                    resParams.Add(value);
            }
            CultureInfo cultureInfo = values[values.Length - 1] as CultureInfo;

            string key = KeyProvider.ProvideKey(resKeyParts);
            object localizedObject = ResManager.Instance.GetResourceObject(key, cultureInfo) ??
                                     ResManager.Instance.GetResourceObject(ResExtension.Key, cultureInfo);
            if (localizedObject == null)
            {
                localizedObject = ResExtension.GetDefaultValue(key);
            }
            else
            {
                try
                {
                    if (resParams.Count > 0)
                        localizedObject = string.Format(localizedObject.ToString(), resParams.Cast<object>().ToArray());
                }
                catch (FormatException)
                {
                }
            }
            return localizedObject;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }

        #endregion
    }
}