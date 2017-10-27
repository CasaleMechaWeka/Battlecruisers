using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

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

            // FELIX  Use BuildableInitialisationArgs?
            BCUtils.IFactoryProvider factoryProvider = Substitute.For<BCUtils.IFactoryProvider>();
			
            IMovementControllerFactory movementControllerFactory = new MovementControllerFactory(null, null);
            factoryProvider.MovementControllerFactory.Returns(movementControllerFactory);

            IFlightPointsProviderFactory flightPointsProviderFactory = new FlightPointsProviderFactory();
            factoryProvider.FlightPointsProviderFactory.Returns(flightPointsProviderFactory);

            nuke.Initialise(nukeStats, targetFilter, target, factoryProvider);
			nuke.Launch();
		}
	}
}
