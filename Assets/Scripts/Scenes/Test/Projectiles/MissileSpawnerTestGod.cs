using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class MissileSpawnerTestGod : MonoBehaviour 
	{
		private MissileSpawnerController _missileSpawner;
		private AircraftController _target;
		private IExactMatchTargetFilter _targetFilter;

		public MissileController missilePrefab;
		public List<Vector2> targetPatrolPoints;

		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			_target = GameObject.FindObjectOfType<AircraftController>();
			helper.InitialiseBuildable(_target);

			_target.CompletedBuildable += (sender, e) => 
			{
				_target.PatrolPoints = targetPatrolPoints;
				_target.StartPatrolling();
			};
			_target.StartConstruction();


			// Setup missile spawner
			_missileSpawner = GameObject.FindObjectOfType<MissileSpawnerController>();
			_targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};

			MissileStats missileStats = new MissileStats(missilePrefab, damage: 50, maxVelocityInMPerS: 20);
			_missileSpawner.Initialise(missileStats, new MovementControllerFactory(), new TargetPositionPredictorFactory());

			InvokeRepeating("FireMissile", time: 0.5f, repeatRate: 0.5f);
		}

		private void FireMissile()
		{
			_missileSpawner.SpawnMissile(angleInDegrees: 45, isSourceMirrored: false, target: _target, targetFilter: _targetFilter);
		}
	}
}
