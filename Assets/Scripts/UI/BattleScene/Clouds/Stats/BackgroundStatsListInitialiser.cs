using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsListInitialiser : MonoBehaviour, IBackgroundStatsProviderInitialiser
    {
        public BackgroundStatsList statsList;

        public IBackgroundStatsProvider CreateProvider(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(statsList);
            statsList.Initialise();
            return statsList;
        }
    }
}