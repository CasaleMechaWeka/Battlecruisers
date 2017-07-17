using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using System;

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
	}
}
