using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    /// <summary>
    /// Factories that are singletons and can be shared between both cruisers.
    /// </summary>
    public interface IPvPFactoryProvider
    {
        IPvPBoostFactory BoostFactory { get; }
        IPvPDamageApplierFactory DamageApplierFactory { get; }
        IPvPDeferrerProvider DeferrerProvider { get; }
        IPvPDroneMonitor DroneMonitor { get; }
        IPvPFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IPvPMovementControllerFactory MovementControllerFactory { get; }
        IPvPPoolProviders PoolProviders { get; }
        IPvPPrefabFactory PrefabFactory { get; }
        IPvPSoundFactoryProvider Sound { get; }
        IPvPSpawnDeciderFactory SpawnDeciderFactory { get; }
        IPvPSpriteChooserFactory SpriteChooserFactory { get; }
        IPvPTargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        IPvPTargetFactoriesProvider Targets { get; }
        IPvPTurretFactoryProvider Turrets { get; }
        IPvPUpdaterProvider UpdaterProvider { get; }
        IPvPSettingsManager SettingsManager { get; }

    }
}
