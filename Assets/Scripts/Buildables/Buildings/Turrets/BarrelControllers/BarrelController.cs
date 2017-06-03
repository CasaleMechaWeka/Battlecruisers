using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
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
		
		protected ITargetFilter _targetFilter;
		protected IAngleCalculator _angleCalculator;

		public ITarget Target { get; set; }
		protected bool IsSourceMirrored { get { return transform.rotation.eulerAngles.y == 180; } }

		private TurretStats _turretStats;
		public TurretStats TurretStats { get { return _turretStats; } }

		public virtual void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator)
		{
			_targetFilter = targetFilter;
			_angleCalculator = angleCalculator;

			_turretStats = gameObject.GetComponent<TurretStats>();
			Assert.IsNotNull(_turretStats);
			_turretStats.Initialise();

			_currentFireIntervalInS = _turretStats.NextFireIntervalInS;
			_timeSinceLastFireInS = float.MaxValue;
		}

		void FixedUpdate()
		{
			if (Target != null)
			{
				_timeSinceLastFireInS += Time.deltaTime;

				Logging.Log(Tags.BARREL_CONTROLLER, "Target.Velocity: " + Target.Velocity);

				float currentAngleInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
				float desiredAngleInDegrees = _angleCalculator.FindDesiredAngle(transform.position, Target, IsSourceMirrored, _turretStats.bulletVelocityInMPerS, currentAngleInRadians);

				bool isOnTarget = IsOnTarget(desiredAngleInDegrees);

				if (!isOnTarget)
				{
					AdjustBarrel(desiredAngleInDegrees);
				}

				if (isOnTarget || _turretStats.IsInBurst)
				{
					if (_timeSinceLastFireInS >= _currentFireIntervalInS)
					{
						// Burst fires happen even if we are no longer on target, so we may miss
						// the target in this case.  Hence use the actual angle our turret barrel
						// is at, intead of the perfect desired angle.
						float fireAngle = _turretStats.IsInBurst ? transform.rotation.eulerAngles.z : desiredAngleInDegrees;

						Fire(fireAngle);

						_timeSinceLastFireInS = 0;
						_currentFireIntervalInS = _turretStats.NextFireIntervalInS;
					}
				}
			}
		}

		protected abstract bool IsOnTarget(float desiredAngleInDegrees);

		protected abstract void AdjustBarrel(float desiredAngleInDegrees);

		protected abstract void Fire(float angleInDegrees);
	}
}
