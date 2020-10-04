using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IBackgroundStatsProviderInitialiser
    {
        IBackgroundStatsProvider CreateProvider(IPrefabFetcher prefabFetcher);
    }
}