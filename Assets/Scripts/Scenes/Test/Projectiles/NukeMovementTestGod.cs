using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class NukeMovementTestGod : MonoBehaviour 
	{
		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			AirFactory target = GameObject.FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target);

			// Setup nuke
			NukeController nuke = GameObject.FindObjectOfType<NukeController>();

			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			NukeStats nukeStats = new NukeStats(nukePrefab: null, damage: 50, maxVelocityInMPerS: 10, cruisingAltitudeInM: 30);
			Vector2 initialVelocity = new Vector2(0, 5);
			IMovementControllerFactory movementControllerFactory = new MovementControllerFactory(null, null);
			IFlightPointsProvider nukeFlightPointsProvider = new NukeFlightPointsProvider();

			nuke.Initialise(nukeStats, initialVelocity, targetFilter, target, movementControllerFactory, nukeFlightPointsProvider);
		}
	}
}
