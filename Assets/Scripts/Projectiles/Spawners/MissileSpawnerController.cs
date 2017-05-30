using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
	public class MissileStats
	{
		private const float INITIAL_VELOCITY_MULTIPLIER = 0.5f;
		
		public MissileController MissilePrefab { get; private set; }
		public float Damage { get; private set; }
		public float MaxVelocityInMPerS { get; private set; }
		public float InitialVelocityInMPerS { get { return MaxVelocityInMPerS * INITIAL_VELOCITY_MULTIPLIER; } }

		public MissileStats(MissileController missilePrefab, float damage, float maxVelocityInMPerS)
		{
			MissilePrefab = missilePrefab;
			Damage = damage;
			MaxVelocityInMPerS = maxVelocityInMPerS;
		}
	}

	public class MissileSpawnerController : MonoBehaviour
	{
		private MissileStats _missileStats;
		private IMovementControllerFactory _movementControllerFactory;
		private ITargetPositionPredictorFactory _targetPositionPredictorFactory;

		public void Initialise(MissileStats missileStats, IMovementControllerFactory movementControllerFactory, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			_missileStats = missileStats;
			_movementControllerFactory = movementControllerFactory;
			_targetPositionPredictorFactory = targetPositionPredictorFactory;
		}

		public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
		{
			MissileController missile = Instantiate<MissileController>(_missileStats.MissilePrefab, transform.position, new Quaternion());
			Vector2 missileVelocity = FindShellVelocity(angleInDegrees, isSourceMirrored, _missileStats.InitialVelocityInMPerS);
			missile.Initialise(target, targetFilter, _missileStats, missileVelocity, _movementControllerFactory, _targetPositionPredictorFactory);
		}

		// FELIX  Avoid duplicate code with ShellSpawnerController
		private Vector2 FindShellVelocity(float angleInDegrees, bool isSourceMirrored, float velocityInMPerS)
		{
			float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

			int xDirectionMultiplier = isSourceMirrored ? -1 : 1;

			float velocityX = velocityInMPerS * Mathf.Cos(angleInRadians) * xDirectionMultiplier;
			float velocityY = velocityInMPerS * Mathf.Sin(angleInRadians);

			Logging.Log(Tags.SHELL_SPAWNER, string.Format("angleInDegrees: {0}  isSourceMirrored: {1}  =>  velocityX: {2}  velocityY: {3}",
				angleInDegrees, isSourceMirrored, velocityX, velocityY));

			return new Vector2(velocityX, velocityY);
		}
	}
}
