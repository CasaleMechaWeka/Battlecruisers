using System.Collections.Generic;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class MissileSpawnerTestGod : MonoBehaviour 
	{
		private MissileSpawner _missileSpawner;
		private TestAircraftController _target;
		private IExactMatchTargetFilter _targetFilter;

		public MissileController missilePrefab;
		public List<Vector2> targetPatrolPoints;

		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			_target = FindObjectOfType<TestAircraftController>();
			_target.PatrolPoints = targetPatrolPoints;
            helper.InitialiseUnit(_target);
            _target.StartConstruction();


			// Setup missile spawner
			_missileSpawner = FindObjectOfType<MissileSpawner>();
			_targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};

            ProjectileStats stats = GetComponent<ProjectileStats>();
            IProjectileStats missileStats = new ProjectileStatsWrapper(stats);
			BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);

            _missileSpawner.Initialise(missileStats, args.FactoryProvider);

			InvokeRepeating("FireMissile", time: 0.5f, repeatRate: 0.5f);
		}

		private void FireMissile()
		{
			_missileSpawner.SpawnMissile(angleInDegrees: 45, isSourceMirrored: false, target: _target, targetFilter: _targetFilter);
		}
	}
}
