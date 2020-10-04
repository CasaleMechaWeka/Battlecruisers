using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsProviderInitialiser : MonoBehaviour, IBackgroundStatsProviderInitialiser
    {
        public IBackgroundStatsProvider CreateProvider(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);
            return new BackgroundStatsProvider(prefabFetcher);
        }
    }
}