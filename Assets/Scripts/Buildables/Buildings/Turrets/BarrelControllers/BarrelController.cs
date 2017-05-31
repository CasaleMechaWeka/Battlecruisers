using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
	/// <summary>
	/// FELIX  Take accuracy into consideration.  Perhaps in AngleCalculator?
	/// </summary>
	public abstract class BarrelController : MonoBehaviour, ITargetConsumer
	{
		private float _currentFireIntervalInS;
		private float _timeSinceLastFireInS;
		
		protected Faction _faction;
		protected IAngleCalculator _angleCalculator;
		protected IMovementControllerFactory _movementControllerFactory;
		protected ITargetPositionPredictorFactory _targetPositionPredictorFactory;
		protected ITargetsFactory _targetsFactory;

		public TurretStats turretStats;

		public ITarget Target { get; set; }
		protected bool IsSourceMirrored { get { return transform.rotation.eulerAngles.y == 180; } }

		public virtual void Initialise(Faction faction, IAngleCalculator angleCalculator, IMovementControllerFactory movementControllerFactory, 
			ITargetPositionPredictorFactory targetPositionPredictorFactory, ITargetsFactory targetsFactory)
		{
			_faction = faction;
			_angleCalculator = angleCalculator;
			_movementControllerFactory = movementControllerFactory;
			_targetPositionPredictorFactory = targetPositionPredictorFactory;
			_targetsFactory = targetsFactory;

			turretStats.Initialise();
			_currentFireIntervalInS = turretStats.NextFireIntervalInS;
			_timeSinceLastFireInS = float.MaxValue;
		}

		// FELIX  Should be FixedUpdate, as it is adjusing the barrle :/
		void Update()
		{
			if (Target != null)
			{
				_timeSinceLastFireInS += Time.deltaTime;

				Logging.Log(Tags.AIRCRAFT, "Target.Velocity: " + Target.Velocity);

				float currentAngleInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
				float desiredAngleInDegrees = _angleCalculator.FindDesiredAngle(transform.position, Target, IsSourceMirrored, turretStats.bulletVelocityInMPerS, currentAngleInRadians);

				bool isOnTarget = IsOnTarget(desiredAngleInDegrees);

				if (!isOnTarget)
				{
					AdjustBarrel(desiredAngleInDegrees);
				}

				if (isOnTarget || turretStats.IsInBurst)
				{
					if (_timeSinceLastFireInS >= _currentFireIntervalInS)
					{
						// Burst fires happen even if we are no longer on target, so we may miss
						// the target in this case.  Hence use the actual angle our turret barrel
						// is at, intead of the perfect desired angle.
						float fireAngle = turretStats.IsInBurst ? transform.rotation.eulerAngles.z : desiredAngleInDegrees;

						Fire(fireAngle);

						_timeSinceLastFireInS = 0;
						_currentFireIntervalInS = turretStats.NextFireIntervalInS;
					}
				}
			}
		}

		protected abstract bool IsOnTarget(float desiredAngleInDegrees);

		protected abstract void AdjustBarrel(float desiredAngleInDegrees);

		protected abstract void Fire(float angleInDegrees);
	}
}
