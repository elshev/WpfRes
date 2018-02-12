using System.Collections.Generic;
using System.Reflection;

namespace Common.Res
{
    /// <summary>
    /// Contains methods for resource related stuff initialization and registration
    /// </summary>
    public abstract class ResRegistrator
    {
        public virtual void Initialize()
        {
            // We use singleton ResManager.Instance for simplicity
            // In real app its better to do this via DI, IoC containers
            RegisterResources(ResManager.Instance);
        }

        public virtual void RegisterResources(ResManager resourceManager)
        {
            foreach (var manager in GetResManagers())
                resourceManager.RegisterResManager(manager);
        }

        protected abstract IEnumerable<IResManager> GetResManagers();
        protected abstract string GetResourceFileName();
        protected abstract Assembly GetExecutingAssembly();

        protected virtual IResManager CreateResManager(string resPrefix, string namespaceSuffix)
        {
            Assembly executingAssembly = GetExecutingAssembly();
            return new FileResManager(resPrefix + "_", GetResourceFileName() + "." + namespaceSuffix, executingAssembly);
        }

        protected virtual IResManager CreateResManager(string resPrefix)
        {
            return CreateResManager(resPrefix, resPrefix);
        }
    }
}