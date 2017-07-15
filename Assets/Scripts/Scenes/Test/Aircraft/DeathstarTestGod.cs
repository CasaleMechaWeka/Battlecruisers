using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Scenes.Test;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
	public class DeathstarTestGod : MonoBehaviour 
	{
		void Start() 
		{
			Helper helper = new Helper();


			// Setup target
			Buildable target = GameObject.FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target, Faction.Blues);


			// Setup deathstar
			Vector2 parentCruiserPosition = new Vector2(-10, 0);
			Vector2 enemyCruiserPosition = new Vector2(10, 0);
			IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition);

			IMovementControllerFactory movementControllerFactory = new MovementControllerFactory(new AngleCalculatorFactory(), new TargetPositionPredictorFactory());

			DeathstarController deathstar = GameObject.FindObjectOfType<DeathstarController>();
			helper.InitialiseBuildable(deathstar, Faction.Reds, aircraftProvider: aircraftProvider, movementControllerFactory: movementControllerFactory);
			deathstar.StartConstruction();
		}
	}
}
