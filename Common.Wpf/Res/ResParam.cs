namespace Common.Wpf.Res
{
    /// <summary>
    /// Lets assign parameter for formatted string in <see cref="ResExtension"/> in XAML
    /// </summary>
    public class ResParam : ResParamBase
    {
        public ResParam()
            : this(null)
        {
        }

        public ResParam(string path)
            : base(path)
        {
        }
    }
}