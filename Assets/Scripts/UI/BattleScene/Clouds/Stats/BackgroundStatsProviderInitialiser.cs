using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsProviderInitialiser : BackgroundStatsProviderInitialiserBase
    {
        public override IBackgroundStatsProvider CreateProvider(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);
            return new BackgroundStatsProvider(prefabFetcher);
        }
    }
}