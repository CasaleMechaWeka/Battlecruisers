using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
	public class MissileSpawner : ProjectileSpawner
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
			Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _missileStats.InitialVelocityInMPerS);
			missile.Initialise(_missileStats, missileVelocity, targetFilter, target, _movementControllerFactory, _targetPositionPredictorFactory);
		}
	}
}
