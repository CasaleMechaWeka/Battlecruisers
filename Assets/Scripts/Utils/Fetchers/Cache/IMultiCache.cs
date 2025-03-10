using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public interface IMultiCache<TPrefab> where TPrefab : class
    {
        TPrefab GetPrefab(IPrefabKey prefabKey);
    }
}