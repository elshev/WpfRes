using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.Res
{
    /// <summary>
    /// Application resource manager
    /// </summary>
    public class ResManager : IResManager
    {
        #region Singleton implementation

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static ResManager()
        {
        }

        private ResManager()
        {
        }

        private static readonly ResManager instance = new ResManager();
        public static ResManager Instance { get { return instance; } }

        #endregion

        private readonly List<IResManager> resManagers = new List<IResManager>();
        private readonly List<IResManager> defaultResManagers = new List<IResManager>();

        /// <summary>
        /// Register resource manager.
        /// </summary>
        /// <param name="resManager">Resource manager.</param>
        public void RegisterResManager(IResManager resManager)
        {
            if (!resManagers.Contains(resManager))
                resManagers.Add(resManager);
        }

        /// <summary>
        /// Register default resource manager.
        /// </summary>
        /// <param name="resManager">Resource manager that will be default.</param>
        public void RegisterDefaultResManager(IResManager resManager)
        {
            if (!defaultResManagers.Contains(resManager))
                defaultResManagers.Add(resManager);
        }

        #region IResManager Members

        /// <summary>
        /// Get resource managers that can find resource by <paramref name="resourceKey"/>
        /// </summary>
        private IEnumerable<IResManager> GetResManagers(string resourceKey)
        {
            return resManagers
                .Where(rm => rm.Match(resourceKey))
                .Union(defaultResManagers);
        }

        /// <summary>
        /// Get resource string by <paramref name="resourceKey"/>
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <returns>
        /// Returns null if resource was not found
        /// </returns>
        public string GetResourceString(string resourceKey)
        {
            return GetResourceString(resourceKey, null);
        }

        /// <summary>
        /// Get resource string by <paramref name="resourceKey"/> and <paramref name="cultureInfo"/>
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <param name="cultureInfo"></param>
        /// <returns>
        /// Returns null if resource was not found
        /// </returns>
        public string GetResourceString(string resourceKey, CultureInfo cultureInfo)
        {
            return GetResManagers(resourceKey)
                .Select(rm => rm.GetResourceString(resourceKey, cultureInfo))
                .FirstOrDefault(res => res != null);
        }

        /// <summary>
        /// Returns resource string for composite key
        /// </summary>
        /// <param name="prefix">Composite key prefix</param>
        /// <param name="parameters">Composite key parts</param>
        public static string GetResourceStringForCompositeKey(string prefix, params object[] parameters)
        {
            string resultKey = ResKeyProvider.GetKey(prefix, parameters);
            return Instance.GetResourceString(resultKey);
        }

        /// <summary>
        /// Получить ресурс по его ключу.
        /// </summary>
        /// <param name="resourceKey">Ключ ресурса.</param>
        /// <returns>Ресурс.</returns>
        public object GetResourceObject(string resourceKey)
        {
            return GetResourceObject(resourceKey, null);
        }

        /// <summary>
        /// Get resource by <paramref name="resourceKey"/> and <paramref name="cultureInfo"/>.
        /// </summary>
        public object GetResourceObject(string resourceKey, CultureInfo cultureInfo)
        {
            return GetResManagers(resourceKey)
                .Select(rm => rm.GetResourceObject(resourceKey, cultureInfo))
                .FirstOrDefault(res => res != null);
        }

        public bool Match(string resourceKey)
        {
            var managers = GetResManagers(resourceKey);
            return managers != null && managers.Any();
        }

        #endregion

        /// <summary>
        /// Get resource string by <paramref name="enumValue"/>
        /// </summary>
        public string GetEnumResource(Enum enumValue)
        {
            return GetResourceString(ResKeyProvider.GetEnumKey(enumValue));
        }
    }
}