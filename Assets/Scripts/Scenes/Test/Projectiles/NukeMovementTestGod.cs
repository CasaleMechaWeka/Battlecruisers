using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
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

            INukeStats nukeStats = GetComponent<NukeProjectileStats>();

            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);

			ITarget parent = Substitute.For<ITarget>();

            nuke.Initialise(args.FactoryProvider);
            nuke.Activate(
                new TargetProviderActivationArgs<INukeStats>(
                    nukeStats,
                    Vector2.zero,
                    targetFilter,
                    parent,
                    target));
            nuke.Launch();
		}
	}
}
