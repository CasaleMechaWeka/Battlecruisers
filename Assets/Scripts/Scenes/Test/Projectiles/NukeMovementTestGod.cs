using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
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

            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);

			ITarget parent = Substitute.For<ITarget>();

            nuke.Initialise(nukeStats, targetFilter, target, args.FactoryProvider, parent);
			nuke.Launch();
		}
	}
}
