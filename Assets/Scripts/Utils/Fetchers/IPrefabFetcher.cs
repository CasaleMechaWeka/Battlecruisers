using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Utils.Fetchers
{
    public interface IPrefabFetcher
    {
        TPrefab GetPrefab<TPrefab>(IPrefabKey prefabKey) where TPrefab : class;
    }
}