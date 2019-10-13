using BattleCruisers.Data.Models.PrefabKeys;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers
{
    public interface IPrefabFetcher
    {
        Task<TPrefab> GetPrefabAsync<TPrefab>(IPrefabKey prefabKey) where TPrefab : class;
    }
}