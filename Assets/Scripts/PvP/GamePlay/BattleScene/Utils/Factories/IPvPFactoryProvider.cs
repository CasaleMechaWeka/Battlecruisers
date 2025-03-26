using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    /// <summary>
    /// Factories that are singletons and can be shared between both cruisers.
    /// </summary>
    public interface IPvPFactoryProvider
    {
        DeferrerProvider DeferrerProvider { get; }
        IDroneMonitor DroneMonitor { get; }
        IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IPvPMovementControllerFactory MovementControllerFactory { get; }
        IPvPPoolProviders PoolProviders { get; }
        IPvPPrefabFactory PrefabFactory { get; }
        ISoundFactoryProvider Sound { get; }
        IPvPSpawnDeciderFactory SpawnDeciderFactory { get; }
        IPvPSpriteChooserFactory SpriteChooserFactory { get; }
        IPvPTargetFactoriesProvider Targets { get; }
        IUpdaterProvider UpdaterProvider { get; }
        SettingsManager SettingsManager { get; }
    }
}
