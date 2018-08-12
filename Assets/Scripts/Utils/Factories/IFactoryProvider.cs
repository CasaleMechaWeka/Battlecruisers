using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Utils.Factories
{
    public interface IFactoryProvider
    {
        ITurretFactoryProvider Turrets { get; }
        ISoundFactoryProvider Sound { get; }
        IAircraftProvider AircraftProvider { get; }
        IBoostFactory BoostFactory { get; }
        IDamageApplierFactory DamageApplierFactory { get; }
        IExplosionFactory ExplosionFactory { get; }
        IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IGlobalBoostProviders GlobalBoostProviders { get; }
        IMovementControllerFactory MovementControllerFactory { get; }
        IPrefabFactory PrefabFactory { get; }
        ISpriteChooserFactory SpriteChooserFactory { get; }
        ITargetsFactory TargetsFactory { get; }
        ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
    }
}
