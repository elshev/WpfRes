using System;
using System.Collections.Generic;
using System.Windows.Markup;
using Common.Res;

namespace Common.Wpf.Res
{
    /// <summary>
    /// Base class for wrappers around key provider.
    /// We need such wrappers for easy using key providers in XAML
    /// </summary>
    public class ResKeyProviderExtension<T> : MarkupExtension, IResKeyProvider where T : class, IResKeyProvider, new()
    {
        protected internal readonly T ResKeyProvider = new T();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public string ProvideKey(IEnumerable<object> parameters)
        {
            return ResKeyProvider.ProvideKey(parameters);
        }
    }

    /// <summary>
    /// Make key from enum: "Entity_EnumeType_EnumValue"
    /// </summary>
    public class EnumResKeyProviderExtension : ResKeyProviderExtension<ResKeyProvider>
    {
        public EnumResKeyProviderExtension()
        {
            ResKeyProvider.BaseKey = Common.Res.ResKeyProvider.EnumPrefix;
        }
    }

}