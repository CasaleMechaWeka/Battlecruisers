using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    /// <summary>
    /// Retrieving an addressable (such as a prefab) is an async operation.  To avoid async
    /// spread, this all prefabs are loaded at once (at the level start).  This means
    /// consuming code can remain synchronous.
    /// </summary>
    public interface IPrefabCacheFactory
    {
        Task<IPrefabCache> CreatePrefabCacheAsync(IPrefabFetcher prefabFetcher);
    }
}