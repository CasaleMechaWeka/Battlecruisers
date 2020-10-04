using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public abstract class BackgroundStatsProviderInitialiserBase : MonoBehaviour, IBackgroundStatsProviderInitialiser
    {
        public abstract IBackgroundStatsProvider CreateProvider(IPrefabFetcher prefabFetcher);
    }
}