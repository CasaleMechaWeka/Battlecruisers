using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Buildables.Units.Aircraft;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class RocketTestGod : MonoBehaviour 
	{
		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			AirFactory target = GameObject.FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target);

			// Setup rockets
			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			RocketStats rocketStats = new RocketStats(rocketPrefab: null, damage: 50, maxVelocityInMPerS: 20, cruisingAltitudeInM: 15);
			Vector2 initialVelocity = new Vector2(5, 5);
			IMovementControllerFactory movementControllerFactory = new MovementControllerFactory();

			RocketController[] rockets = GameObject.FindObjectsOfType<RocketController>() as RocketController[];
			foreach (RocketController rocket in rockets)
			{
				rocket.Initialise(rocketStats, initialVelocity, targetFilter, target, movementControllerFactory);
			}
		}
	}
}
