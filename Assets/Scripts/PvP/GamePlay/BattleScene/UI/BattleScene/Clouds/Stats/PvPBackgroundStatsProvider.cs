using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public class PvPBackgroundStatsProvider : IPvPBackgroundStatsProvider
    {
        private readonly IPvPPrefabFetcher _prefabFetcher;

        public PvPBackgroundStatsProvider(IPvPPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);
            _prefabFetcher = prefabFetcher;
        }

        public async Task<IPvPPrefabContainer<PvPBackgroundImageStats>> GetStatsAsync(int levelNum)
        {
            IPvPPrefabKey key = new PvPBackgroundImageStatsKey(levelNum);
            return await _prefabFetcher.GetPrefabAsync<PvPBackgroundImageStats>(key);
        }
    }
}