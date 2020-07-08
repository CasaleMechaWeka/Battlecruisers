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
    public class RocketMovementTestGod : TestGodBase
	{
        private AirFactory _target;
        private RocketController _rocket;

        public AudioClip impactSound;

        protected override List<GameObject> GetGameObjects()
        {
			_target = FindObjectOfType<AirFactory>();
			_rocket = FindObjectOfType<RocketController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _rocket.gameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            Assert.IsNotNull(impactSound);

			// Setup target
            helper.InitialiseBuilding(_target);

            // Setup rocket
            ICruisingProjectileStats rocketStats = GetComponent<CruisingProjectileStats>();
            
            Vector2 initialVelocity = new Vector2(0, 5);

			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};
			
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);

            ITarget parent = Substitute.For<ITarget>();
            parent.Faction.Returns(Faction.Blues);

            _rocket.Initialise(args.FactoryProvider);
            _rocket.Activate(
                new TargetProviderActivationArgs<ICruisingProjectileStats>(
                    _rocket.Position,
                    rocketStats,
                    initialVelocity,
                    targetFilter,
                    parent,
                    new AudioClipWrapper(impactSound),
                    _target));
        }
	}
}
