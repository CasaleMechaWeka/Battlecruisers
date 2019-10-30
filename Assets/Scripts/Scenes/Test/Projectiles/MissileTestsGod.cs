using BattleCruisers.Buildables;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class MissileTestsGod : TestGodBase
	{
        private MissileController[] _missiles;

        protected override List<GameObject> GetGameObjects()
        {
            _missiles = FindObjectsOfType<MissileController>();
            return
                _missiles
                    .Select(missile => missile.gameObject)
                    .ToList();
        }

        protected void SetupMissiles(Helper helper, ITarget target)
		{
			// Setup missiles
			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};

            IProjectileStats missileStats = GetComponent<ProjectileStats>();
			Vector2 initialVelocity = new Vector2(5, 5);
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);
            ITarget parent = Substitute.For<ITarget>();

            foreach (MissileController missile in _missiles)
			{
                missile.Initialise(args.FactoryProvider);
                missile.Activate(
                    new TargetProviderActivationArgs<IProjectileStats>(
                        missile.Position,
                        missileStats,
                        initialVelocity,
                        targetFilter,
                        parent,
                        target));
            }
		}
	}
}
