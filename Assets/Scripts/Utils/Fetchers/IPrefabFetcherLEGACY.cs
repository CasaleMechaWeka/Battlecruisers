using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Utils.Fetchers
{
    public interface IPrefabFetcherLEGACY
    {
        TPrefab GetPrefab<TPrefab>(IPrefabKey prefabKey) where TPrefab : class;
    }
}