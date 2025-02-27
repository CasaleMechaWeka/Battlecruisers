using BattleCruisers.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    /// <summary>
    /// Factories that are singletons and can be shared between both cruisers.
    /// </summary>
    public interface IPvPFactoryProvider
    {
        IBoostFactory BoostFactory { get; }
        IDamageApplierFactory DamageApplierFactory { get; }
        IDeferrerProvider DeferrerProvider { get; }
        IPvPDroneMonitor DroneMonitor { get; }
        IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IPvPMovementControllerFactory MovementControllerFactory { get; }
        IPvPPoolProviders PoolProviders { get; }
        IPvPPrefabFactory PrefabFactory { get; }
        ISoundFactoryProvider Sound { get; }
        IPvPSpawnDeciderFactory SpawnDeciderFactory { get; }
        IPvPSpriteChooserFactory SpriteChooserFactory { get; }
        ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        IPvPTargetFactoriesProvider Targets { get; }
        ITurretFactoryProvider Turrets { get; }
        IPvPUpdaterProvider UpdaterProvider { get; }
        ISettingsManager SettingsManager { get; }
    }
}
