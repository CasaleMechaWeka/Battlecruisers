using System.Collections.Generic;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
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
			helper.InitialiseBuildable(_target);
            _target.StartConstruction();


			// Setup missile spawner
			_missileSpawner = FindObjectOfType<MissileSpawner>();
			_targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};

			MissileStats missileStats = new MissileStats(missilePrefab, damage: 50, maxVelocityInMPerS: 20);
			_missileSpawner.Initialise(missileStats, new MovementControllerFactory(null, null), new TargetPositionPredictorFactory());

			InvokeRepeating("FireMissile", time: 0.5f, repeatRate: 0.5f);
		}

		private void FireMissile()
		{
			_missileSpawner.SpawnMissile(angleInDegrees: 45, isSourceMirrored: false, target: _target, targetFilter: _targetFilter);
		}
	}
}
