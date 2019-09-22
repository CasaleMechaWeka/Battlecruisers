using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils.Factories
{
    /// <summary>
    /// Factories that are singletons and can be shared between both cruisers.
    /// </summary>
    public interface IFactoryProvider
    {
        IBoostFactory BoostFactory { get; }
        IDamageApplierFactory DamageApplierFactory { get; }
        IDeferrerProvider DeferrerProvider { get; }
        IDroneAudioActivenessDecider DroneAudioActivenessDecider { get; }
        IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IMovementControllerFactory MovementControllerFactory { get; }
        IPoolProviders PoolProviders { get; }
        IPrefabFactory PrefabFactory { get; }
        ISoundFactoryProvider Sound { get; }
        ISpawnDeciderFactory SpawnDeciderFactory { get; }
        ISpriteChooserFactory SpriteChooserFactory { get; }
        ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        ITargetFactoriesProvider Targets { get; }
        ITurretFactoryProvider Turrets { get; }
        IUpdaterProvider UpdaterProvider { get; }
    }
}
