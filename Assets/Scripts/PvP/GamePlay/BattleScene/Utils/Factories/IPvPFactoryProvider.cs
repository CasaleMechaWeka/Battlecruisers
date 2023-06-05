using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
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
        // ISettingsManager SettingsManager { get; }

    }
}
