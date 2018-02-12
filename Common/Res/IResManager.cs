using System.Globalization;

namespace Common.Res
{
    /// <summary> Resource manager </summary>
    public interface IResManager
    {
        /// <summary>
        /// Get string from resources by key depending on current culture.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <returns>String from resources.</returns>
        string GetResourceString(string resourceKey);

        /// <summary>
        /// Get string from resources by key depending on given culture.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <param name="cultureInfo"></param>
        /// <returns>String from resources.</returns>
        string GetResourceString(string resourceKey, CultureInfo cultureInfo);

        /// <summary>
        /// Get object from resources by key depending on current culture.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <returns>Object from resources.</returns>
        object GetResourceObject(string resourceKey);

        /// <summary>
        /// Get object from resources by key depending on given culture.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <param name="cultureInfo"></param>
        /// <returns>Object from resources.</returns>
        object GetResourceObject(string resourceKey, CultureInfo cultureInfo);

        /// <summary>
        /// Returns true if resource manager contains resource for <paramref name="resourceKey"/>.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        bool Match(string resourceKey);
    }
}