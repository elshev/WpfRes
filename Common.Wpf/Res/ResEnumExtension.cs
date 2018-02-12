using Common.Res;

namespace Common.Wpf.Res
{
    public class ResEnumExtension : ResExtension
    {
        protected override IResKeyProvider CreateDefaultKeyProvider()
        {
            return new ResKeyProvider(ResKeyProvider.EnumPrefix);
        }
    }
}