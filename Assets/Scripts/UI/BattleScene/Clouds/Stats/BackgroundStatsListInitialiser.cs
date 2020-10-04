using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsListInitialiser : BackgroundStatsProviderInitialiserBase
    {
        public BackgroundStatsList statsList;

        public override IBackgroundStatsProvider CreateProvider(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(statsList);
            statsList.Initialise();
            return statsList;
        }
    }
}