using System.Collections.Generic;
using System.Reflection;
using Common.Res;

namespace ResApp.Resources
{
    /// <summary>
    /// Resource initialization for this app.
    /// </summary>
    public class AppResRegistrator : ResRegistrator
    {
        protected override string GetResourceFileName()
        {
            return "ResApp.Resources";
        }

        protected override Assembly GetExecutingAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        protected override IEnumerable<IResManager> GetResManagers()
        {
            return new[]
            {
                CreateResManager("Entity"),
                CreateResManager("Lib")
            };
        }

        public override void Initialize()
        {
            base.Initialize();
            var defaultResManager = new FileResManager(GetResourceFileName() + "." + "ResApp", Assembly.GetExecutingAssembly());
            ResManager.Instance.RegisterDefaultResManager(defaultResManager);
        }
    }
}
