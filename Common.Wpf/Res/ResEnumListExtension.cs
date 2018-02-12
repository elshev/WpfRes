using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using Common.Res;

namespace Common.Wpf.Res
{
    /// <summary>
    /// MarkupExtension that allows to use Enum list in XAML.
    /// </summary>
    public class ResEnumListExtension : MarkupExtension
    {
        public ResEnumListExtension(Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException("enumType");
            EnumType = enumType;
        }

        private Type enumType;
        public Type EnumType
        {
            get { return enumType; }
            private set
            {
                if (enumType == value) return;
                var type = Nullable.GetUnderlyingType(value) ?? value;
                if (type.IsEnum == false) throw new ArgumentException("Type must be an Enum.");
                enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var converter = new ResEnumConverter();
            Binding binding = new Binding("UICulture")
            {
                Source = CultureManager.Instance,
                Mode = BindingMode.OneWay,
                Converter = converter,
                ConverterParameter = EnumType
            };
            return binding.ProvideValue(serviceProvider);
        }

        #region ResEnumConverter

        private class ResEnumConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Type enumType = parameter as Type;
                if (enumType == null || !enumType.IsEnum) return null;
                IEnumerable<object> enumValues = Enum
                    .GetValues(enumType)
                    .Cast<Enum>()
                    .Select(enumValue => ResManager.Instance.GetEnumResource(enumValue))
                    .ToArray();
                return enumValues;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return Binding.DoNothing;
            }
        }

        #endregion
    }
}