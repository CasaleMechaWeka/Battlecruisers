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
		private FireIntervalManager _fireIntervalManager;
		
		protected ITargetFilter _targetFilter;
		protected IAngleCalculator _angleCalculator;

		public ITarget Target { get; set; }
		protected bool IsSourceMirrored { get { return transform.IsMirrored(); } }

		private TurretStats _turretStats;
		public TurretStats TurretStats { get { return _turretStats; } }

		public virtual void StaticInitialise()
		{
			_turretStats = gameObject.GetComponent<TurretStats>();
			Assert.IsNotNull(_turretStats);
			_turretStats.Initialise();
			
			_fireIntervalManager = gameObject.GetComponent<FireIntervalManager>();
			Assert.IsNotNull(_fireIntervalManager);
			_fireIntervalManager.Initialise(_turretStats);
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
				float desiredAngleInDegrees = _angleCalculator.FindDesiredAngle(transform.position, Target, IsSourceMirrored, _turretStats.bulletVelocityInMPerS, currentAngleInRadians);

				bool isOnTarget = IsOnTarget(desiredAngleInDegrees);

				if (!isOnTarget)
				{
					AdjustBarrel(desiredAngleInDegrees);
				}

				if (isOnTarget || _turretStats.IsInBurst)
				{
					if (_fireIntervalManager.IsIntervalUp())
					{
						// Burst fires happen even if we are no longer on target, so we may miss
						// the target in this case.  Hence use the actual angle our turret barrel
						// is at, intead of the perfect desired angle.
						float fireAngle = _turretStats.IsInBurst ? transform.rotation.eulerAngles.z : desiredAngleInDegrees;

						Fire(fireAngle);
					}
				}
			}
		}

		protected abstract bool IsOnTarget(float desiredAngleInDegrees);

		protected abstract void AdjustBarrel(float desiredAngleInDegrees);

		protected abstract void Fire(float angleInDegrees);
	}
}
