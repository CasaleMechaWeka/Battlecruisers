using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
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

            CruisingProjectileStats stats = GetComponent<CruisingProjectileStats>();
            INukeStats nukeStats = new NukeStatsWrapper(stats);
            
			IMovementControllerFactory movementControllerFactory = new MovementControllerFactory(null, null);
			IFlightPointsProvider nukeFlightPointsProvider = new NukeFlightPointsProvider();
			IDamageApplier damageApplier = new SingleDamageApplier(nukeStats.Damage);

			nuke.Initialise(nukeStats, targetFilter, damageApplier, target, movementControllerFactory, nukeFlightPointsProvider);
			nuke.Launch();
		}
	}
}
