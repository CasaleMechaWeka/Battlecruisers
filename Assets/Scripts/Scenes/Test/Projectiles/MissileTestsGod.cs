using BattleCruisers.Buildables;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class MissileTestsGod : MonoBehaviour 
	{
		protected void SetupMissiles(ITarget target)
		{
			// Setup missiles
			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};

            IProjectileStats missileStats = GetComponent<ProjectileStats>();
			Vector2 initialVelocity = new Vector2(5, 5);
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(new Helper());
            ITarget parent = Substitute.For<ITarget>();

            MissileController[] missiles = FindObjectsOfType<MissileController>();

            foreach (MissileController missile in missiles)
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
