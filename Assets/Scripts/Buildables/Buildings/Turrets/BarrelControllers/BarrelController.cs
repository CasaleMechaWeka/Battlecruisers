using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
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
		protected IFireIntervalManager _fireIntervalManager;
		
		protected ITargetFilter _targetFilter;
		protected IAngleCalculator _angleCalculator;

		public ITarget Target { get; set; }
		protected bool IsSourceMirrored { get { return transform.IsMirrored(); } }

		public virtual TurretStats TurretStats { get; protected set; }

		public virtual void StaticInitialise()
		{
			// Turret stats
			TurretStats = gameObject.GetComponent<TurretStats>();
			Assert.IsNotNull(TurretStats);
			TurretStats.Initialise();

			// Fire interval manager
			FireIntervalManager fireIntervalManager = gameObject.GetComponent<FireIntervalManager>();
			Assert.IsNotNull(_fireIntervalManager);
			fireIntervalManager.Initialise(TurretStats);
			_fireIntervalManager = fireIntervalManager;
		}

		public virtual void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator)
		{
			_targetFilter = targetFilter;
			_angleCalculator = angleCalculator;
		}

		void FixedUpdate()
		{
			if (Target != null)
			{
				Logging.Log(Tags.BARREL_CONTROLLER, "Target.Velocity: " + Target.Velocity);

				float currentAngleInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
				float desiredAngleInDegrees = _angleCalculator.FindDesiredAngle(transform.position, Target, IsSourceMirrored, TurretStats.bulletVelocityInMPerS, currentAngleInRadians);

				bool isOnTarget = IsOnTarget(desiredAngleInDegrees);

				if (!isOnTarget)
				{
					AdjustBarrel(desiredAngleInDegrees);
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

		protected abstract bool IsOnTarget(float desiredAngleInDegrees);

		protected abstract void AdjustBarrel(float desiredAngleInDegrees);

		protected abstract void Fire(float angleInDegrees);

		protected virtual void CeaseFire() { }
	}
}
