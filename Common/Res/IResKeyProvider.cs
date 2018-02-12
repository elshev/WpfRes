using System.Collections.Generic;


namespace Common.Res
{
    /// <summary>
    /// Provides key for Resource Managers
    /// </summary>
    public interface IResKeyProvider
    {
        /// <summary>
        /// Provides key for Resource Managers.
        /// Key can be composed from <paramref name="parameters"/>
        ///  </summary>
        string ProvideKey(IEnumerable<object> parameters);
    }
}
