using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;

namespace BattleCruisers.Utils
{
    public interface IFactoryProvider
	{
		IPrefabFactory PrefabFactory { get; }
		ITargetsFactory TargetsFactory { get; }
		IMovementControllerFactory MovementControllerFactory { get; }
		IAngleCalculatorFactory AngleCalculatorFactory { get; }
		ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
		IAircraftProvider AircraftProvider { get; }
		IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IBoostProvidersManager BoostProvidersManager { get; }
        IBoostFactory BoostFactory { get; }
        IDamageApplierFactory DamageApplierFactory { get; }
        IExplosionFactory ExplosionFactory { get; }
        IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
	}
}
