using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
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
	}

	public class FactoryProvider : IFactoryProvider
	{
		public IPrefabFactory PrefabFactory { get; private set; }
		public ITargetsFactory TargetsFactory { get; private set; }
		public IMovementControllerFactory MovementControllerFactory { get; private set; }
		public IAngleCalculatorFactory AngleCalculatorFactory { get; private set; }
		public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; private set; }
		public IAircraftProvider AircraftProvider { get; private set; }

		public FactoryProvider(PrefabFactory prefabFactory, ICruiser friendlyCruiser, ICruiser enemyCruiser)
		{
			PrefabFactory = prefabFactory;
			TargetsFactory = new TargetsFactory(enemyCruiser);
			AngleCalculatorFactory = new AngleCalculatorFactory();
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory(AngleCalculatorFactory, TargetPositionPredictorFactory);
			AircraftProvider = new AircraftProvider(friendlyCruiser.Position, enemyCruiser.Position);
		}
	}
}

