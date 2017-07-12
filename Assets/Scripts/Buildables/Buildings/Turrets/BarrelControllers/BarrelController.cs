using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Velocity;
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
		protected IFireIntervalManager _fireIntervalManager;
		
		protected ITargetFilter _targetFilter;
		protected IAngleCalculator _angleCalculator;
		protected IRotationMovementController _rotationMovementController;

		public ITarget Target { get; set; }
		protected bool IsSourceMirrored { get { return transform.IsMirrored(); } }

		public virtual TurretStats TurretStats { get; protected set; }
		private bool IsInitialised { get { return _targetFilter != null; } }

		public virtual void StaticInitialise()
		{
			// Turret stats
			TurretStats = gameObject.GetComponent<TurretStats>();
			Assert.IsNotNull(TurretStats);
			TurretStats.Initialise();

			// Fire interval manager
			FireIntervalManager fireIntervalManager = gameObject.GetComponent<FireIntervalManager>();
			Assert.IsNotNull(fireIntervalManager);
			fireIntervalManager.Initialise(TurretStats);
			_fireIntervalManager = fireIntervalManager;
		}

		public virtual void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator, IRotationMovementController rotationMovementController)
		{
			_targetFilter = targetFilter;
			_angleCalculator = angleCalculator;
			_rotationMovementController = rotationMovementController;
		}

		void FixedUpdate()
		{
			if (!IsInitialised)
			{
				return;
			}

			if (Target != null)
			{
				Logging.Log(Tags.BARREL_CONTROLLER, "Target.Velocity: " + Target.Velocity);

				float currentAngleInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
				float desiredAngleInDegrees = _angleCalculator.FindDesiredAngle(transform.position, Target, IsSourceMirrored, TurretStats.bulletVelocityInMPerS, currentAngleInRadians);

				bool isOnTarget = _rotationMovementController.IsOnTarget(desiredAngleInDegrees);

				if (!isOnTarget)
				{
					_rotationMovementController.AdjustRotation(desiredAngleInDegrees);
				}

				if ((isOnTarget || TurretStats.IsInBurst)
				    && _fireIntervalManager.IsIntervalUp())
				{
					// Burst fires happen even if we are no longer on target, so we may miss
					// the target in this case.  Hence use the actual angle our turret barrel
					// is at, intead of the perfect desired angle.
					float fireAngle = TurretStats.IsInBurst ? transform.rotation.eulerAngles.z : desiredAngleInDegrees;

					Fire(fireAngle);
				}
				else
				{
					CeaseFire();
				}
			}
			else
			{
				CeaseFire();
			}
		}

		protected abstract void Fire(float angleInDegrees);

		protected virtual void CeaseFire() { }
	}
}
