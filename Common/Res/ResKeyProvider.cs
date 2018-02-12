using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common.Res
{
    /// <summary>
    /// Default key provider.
    /// Makes up a key as: "BaseKey_Type1_Value1_Type2_Value2"
    /// </summary>
    public class ResKeyProvider : IResKeyProvider
    {
        private const string NoKey = "<NoKey>";
        public const string EnumPrefix = "Entity";

        public string BaseKey { get; set; }
        public ResKeyProvider()
        {
            BaseKey = NoKey;
        }

        public ResKeyProvider(string baseKey)
        {
            BaseKey = baseKey ?? NoKey;
        }

        public static string GetKey(string prefix, IEnumerable<object> parameters)
        {
            if (parameters == null || !parameters.Any()) return prefix;
            var stringBuilder = new StringBuilder(prefix);
            foreach (object parameter in parameters)
            {
                if (parameter is CultureInfo) continue;
                if (parameter != null)
                    stringBuilder.AppendFormat("_{0}_{1}", parameter.GetType().Name, parameter);
            }
            return stringBuilder.ToString();
        }

        public string ProvideKey(IEnumerable<object> parameters)
        {
            return GetKey(BaseKey, parameters);
        }

        /// <summary>
        /// Get resource key for enum value
        /// Makes up a key as: "Enum_EnumType_EnumValue"
        /// </summary>
        public static string GetEnumKey(Enum enumValue)
        {
            return GetKey(EnumPrefix, new object[] { enumValue });
        }
    }
}