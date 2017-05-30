using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
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
		private Faction _faction;
		protected IAngleCalculator _angleCalculator;
		private float _currentFireIntervalInS;
		private float _timeSinceLastFireInS;
		private ShellStats _shellStats;

		public ShellSpawnerController shellSpawner;
		public TurretStats turretStats;

		public ITarget Target { get; set; }
		private bool IsSourceMirrored { get { return transform.rotation.eulerAngles.y == 180; } }

		public void Initialise(Faction faction, IAngleCalculator angleCalculator)
		{
			_faction = faction;
			_angleCalculator = angleCalculator;

			turretStats.Initialise();
			_currentFireIntervalInS = turretStats.NextFireIntervalInS;
			_timeSinceLastFireInS = float.MaxValue;
			_shellStats = new ShellStats(turretStats.shellPrefab, turretStats.damage, turretStats.ignoreGravity, turretStats.bulletVelocityInMPerS);
			shellSpawner.Initialise(_faction, _shellStats);
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

		private void Fire(float angleInDegrees)
		{
			Logging.Log(Tags.BARREL_CONTROLLER, "Fire()  angleInDegrees: " + angleInDegrees);
			shellSpawner.SpawnShell(angleInDegrees, IsSourceMirrored);
		}
	}
}
