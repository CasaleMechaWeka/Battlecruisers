using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles;
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
		private float _currentFireIntervalInS;
		private float _timeSinceLastFireInS;
		private ShellStats _shellStats;

		public ShellSpawnerController shellSpawner;
		public TurretStats turretStats;
		public AngleCalculator angleCalculator;

		public ITarget Target { get; set; }
		private bool IsSourceMirrored { get { return transform.rotation.eulerAngles.y == 180; } }

		public void Initialise(Faction faction)
		{
			_faction = faction;
			turretStats.Initialise();
			_currentFireIntervalInS = turretStats.NextFireIntervalInS;
			_timeSinceLastFireInS = float.MaxValue;
			_shellStats = new ShellStats(turretStats.shellPrefab, turretStats.damage, turretStats.ignoreGravity, turretStats.bulletVelocityInMPerS);
			shellSpawner.Initialise(_faction, _shellStats);
		}

		void Update()
		{
			if (Target != null)
			{
				_timeSinceLastFireInS += Time.deltaTime;

				Vector2 sourcePosition = new Vector2(transform.position.x, transform.position.y);
				Vector3 targetPositionV3 = Target.GameObject.transform.position;
				Vector2 targetPosition = new Vector2(targetPositionV3.x, targetPositionV3.y);

				Logging.Log(Tags.AIRCRAFT, "Target.Velocity: " + Target.Velocity);

				float desiredAngleInDegrees = angleCalculator.FindDesiredAngle(sourcePosition, targetPosition, IsSourceMirrored, turretStats.bulletVelocityInMPerS, Target.Velocity);

				bool isOnTarget = IsOnTarget(desiredAngleInDegrees);

				if (!isOnTarget)
				{
					AdjustBarrel(desiredAngleInDegrees);
				}

				if (isOnTarget || turretStats.IsInBurst)
				{
					if (_timeSinceLastFireInS >= _currentFireIntervalInS)
					{
						Fire(desiredAngleInDegrees);

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
