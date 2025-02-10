using BattleCruisers.Data.Models.PrefabKeys;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public interface IPvPPrefabFetcher
    {
        Task<IPvPPrefabContainer<TPrefab>> GetPrefabAsync<TPrefab>(IPrefabKey prefabKey) where TPrefab : class;
    }
}