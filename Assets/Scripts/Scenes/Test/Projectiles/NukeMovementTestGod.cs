using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class NukeMovementTestGod : MonoBehaviour 
	{
		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target);

			// Setup nuke
			NukeController nuke = FindObjectOfType<NukeController>();

			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
            NukeStats nukeStats = new NukeStats(nukePrefab: null, damage: 50, maxVelocityInMPerS: 10, cruisingAltitudeInM: 30, damageRadiusInM: 10);
			Vector2 initialVelocity = new Vector2(0, 5);
			IMovementControllerFactory movementControllerFactory = new MovementControllerFactory(null, null);
			IFlightPointsProvider nukeFlightPointsProvider = new NukeFlightPointsProvider();
			IDamageApplier damageApplier = new SingleDamageApplier(nukeStats.Damage);

			nuke.Initialise(nukeStats, initialVelocity, targetFilter, damageApplier, target, movementControllerFactory, nukeFlightPointsProvider);
			nuke.Launch();
		}
	}
}
