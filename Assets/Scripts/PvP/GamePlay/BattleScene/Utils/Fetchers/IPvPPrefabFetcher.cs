using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public interface IPvPPrefabFetcher
    {
        Task<IPrefabContainer<TPrefab>> GetPrefabAsync<TPrefab>(IPrefabKey prefabKey) where TPrefab : class;
    }
}