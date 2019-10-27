using System.Collections.Generic;
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
    public class NukeMovementTestGod : TestGodBase
	{
        private AirFactory _target;
        private NukeController _nuke;

        protected override List<GameObject> GetGameObjects()
        {
			_target = FindObjectOfType<AirFactory>();
			_nuke = FindObjectOfType<NukeController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _nuke.gameObject
            };
        }

        protected override void Setup(Helper helper)
        {
			// Setup target
            helper.InitialiseBuilding(_target);

			// Setup nuke
			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};

            INukeStats nukeStats = GetComponent<NukeProjectileStats>();

            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);

			ITarget parent = Substitute.For<ITarget>();

            _nuke.Initialise(args.FactoryProvider);
            _nuke.Activate(
                new TargetProviderActivationArgs<INukeStats>(
                    _nuke.Position,
                    nukeStats,
                    Vector2.zero,
                    targetFilter,
                    parent,
                    _target));
            _nuke.Launch();
		}
	}
}
