using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class RocketMovementTestGod : MonoBehaviour 
	{
		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			AirFactory target = FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target);

			// Setup rocket
			RocketController rocket = FindObjectOfType<RocketController>();

			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			RocketStats rocketStats = new RocketStats(rocketPrefab: null, damage: 50, maxVelocityInMPerS: 10, cruisingAltitudeInM: 25);
			Vector2 initialVelocity = new Vector2(0, 5);
			IMovementControllerFactory movementControllerFactory = new MovementControllerFactory(null, null);

			rocket.Initialise(rocketStats, initialVelocity, targetFilter, target, movementControllerFactory, Faction.Blues, new RocketFlightPointsProvider());
		}
	}
}
