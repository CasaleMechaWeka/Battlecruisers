using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    // FELIX  Remove :)  Check CloudV2Test scene!
    public interface IBackgroundStatsProviderInitialiser
    {
        IBackgroundStatsProvider CreateProvider(IPrefabFetcher prefabFetcher);
    }
}