using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    // FELIX  Remove :)
    public interface IBackgroundStatsProviderInitialiser
    {
        IBackgroundStatsProvider CreateProvider(IPrefabFetcher prefabFetcher);
    }
}