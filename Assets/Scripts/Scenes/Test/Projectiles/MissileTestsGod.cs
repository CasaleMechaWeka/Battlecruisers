using BattleCruisers.Buildables;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
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

            ProjectileStats stats = GetComponent<ProjectileStats>();
            IProjectileStats missileStats = new ProjectileStatsWrapper(stats);
			Vector2 initialVelocity = new Vector2(5, 5);
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(new Helper());

            MissileController[] missiles = FindObjectsOfType<MissileController>();

            foreach (MissileController missile in missiles)
			{
                missile.Initialise(missileStats, initialVelocity, targetFilter, target, args.FactoryProvider);
			}
		}
	}
}
