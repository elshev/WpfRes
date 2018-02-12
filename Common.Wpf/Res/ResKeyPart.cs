namespace Common.Wpf.Res
{
    /// <summary>
    /// Created for convenient assigning key part for <see cref="ResExtension"/> key in XAML
    /// </summary>
    public class ResKeyPart : ResParamBase
    {
        public string Key { get; set; }
        public ResKeyPart()
            : this(null)
        {
        }

        public ResKeyPart(string path)
            : base(path)
        {
        }
    }
}