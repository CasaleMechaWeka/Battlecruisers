using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils.Factories
{
    public interface IFactoryProvider
    {
        IPoolProviders PoolProviders { get; }
        ISoundFactoryProvider Sound { get; }
        ITurretFactoryProvider Turrets { get; }
        IAircraftProvider AircraftProvider { get; }
        IBoostFactory BoostFactory { get; }
        IDamageApplierFactory DamageApplierFactory { get; }
        IDeferrerProvider DeferrerProvider { get; }

        // FELIX  Create Pools sub provider, once have UnitsPools too :)
        IExplosionPoolProvider ExplosionPoolProvider { get; }

        IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IGlobalBoostProviders GlobalBoostProviders { get; }
        IMovementControllerFactory MovementControllerFactory { get; }
        IPrefabFactory PrefabFactory { get; }
        ISpawnDeciderFactory SpawnDeciderFactory { get; }
        ISpriteChooserFactory SpriteChooserFactory { get; }
        ITargetFactoriesProvider TargetFactories { get; }
        ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        ITrackerFactory TrackerFactory { get; }
        IUpdaterProvider UpdaterProvider { get; }
    }
}
