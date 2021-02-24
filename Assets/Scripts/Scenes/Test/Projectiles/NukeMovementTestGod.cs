using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class NukeMovementTestGod : TestGodBase
	{
        private AirFactory _target;
        private NukeController _nuke;

        public AudioClip impactSound;

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
            Assert.IsNotNull(impactSound);

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

            _nuke.Initialise(helper.CommonStrings, args.FactoryProvider);
            _nuke.Activate(
                new TargetProviderActivationArgs<INukeStats>(
                    _nuke.Position,
                    nukeStats,
                    Vector2.zero,
                    targetFilter,
                    parent,
                    new AudioClipWrapper(impactSound),
                    _target));
            _nuke.Launch();
		}
	}
}
