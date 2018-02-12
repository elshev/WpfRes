using System.Windows.Data;

namespace Common.Wpf.Res
{
    /// <summary>
    /// Base class for different kinds of parameters for <see cref="ResExtension"/>
    /// </summary>
    public abstract class ResParamBase : Binding
    {
        protected ResParamBase()
            : this(null)
        {
        }

        protected ResParamBase(string path)
            : base(path)
        {
            Mode = BindingMode.OneWay;
            FallbackValue = string.Empty;
        }
    }
}