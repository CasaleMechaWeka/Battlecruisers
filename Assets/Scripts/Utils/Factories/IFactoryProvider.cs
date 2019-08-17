using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
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
    public interface IFactoryProvider
    {
        // FELIX  Sort alphabetically again :)
        // Common
        IBoostFactory BoostFactory { get; }
        IDeferrerProvider DeferrerProvider { get; }
        IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        ISpriteChooserFactory SpriteChooserFactory { get; }
        IMovementControllerFactory MovementControllerFactory { get; }
        IPrefabFactory PrefabFactory { get; }
        ISpawnDeciderFactory SpawnDeciderFactory { get; }
        ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        IUpdaterProvider UpdaterProvider { get; }
        ISoundFactoryProvider Sound { get; }
        ITurretFactoryProvider Turrets { get; }
        ITargetFactoriesProvider TargetFactories { get; }

        // Common, but circular dependency :/
        IPoolProviders PoolProviders { get; } // IFactoryProvider :/
        IDamageApplierFactory DamageApplierFactory { get; } // FilterFactory => TargetsFactories => cruisers :/
    }
}
