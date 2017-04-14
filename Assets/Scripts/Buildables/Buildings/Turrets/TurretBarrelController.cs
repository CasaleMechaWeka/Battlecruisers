using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	/// <summary>
	/// For turrets whose projectiles are not affected by gravity.  Ie, they fly in a straight line
	/// from the barrel tip to the enemy object.  
	/// FELIX  Lead moving targets!
	/// FELIX  Take accuracy into consideration
	/// </summary>
	public class TurretBarrelController : MonoBehaviour 
	{
		private float _timeSinceLastFireInS;
		private ShellStats _shellStats;
		protected float _maxRange;

		// FELIX  Allow to vary depending on artillery?  Perhaps also part of TurretStats?
		public Rigidbody2D shellPrefab;
		public ShellSpawnerController shellSpawner;
		public TurretStats turretStats;

		public GameObject Target { get; set; }

		// FELIX  Add this rotate speed to turret stats
		private const float ROTATE_SPEED_IN_DEGREES_PER_S = 5;
		private const float ROTATION_EQUALITY_MARGIN_IN_RADIANS = 0.01f;

		void Awake()
		{
			_maxRange = FindMaxRange(turretStats.bulletVelocityInMPerS);
			_timeSinceLastFireInS = float.MaxValue;
			_shellStats = new ShellStats(shellPrefab, turretStats.damage, turretStats.ignoreGravity, turretStats.bulletVelocityInMPerS);
			shellSpawner.Initialise(_shellStats);
		}
		
		/// <summary>
		/// Assumes no y axis difference in source and target
		/// </summary>
		private float FindMaxRange(float velocityInMPerS)
		{
			if (turretStats.ignoreGravity)
			{
				return float.MaxValue;
			}
			else
			{
				return (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;
			}
		}

		void Update()
		{
			if (Target != null)
			{
				float desiredAngleInRadians = FindDesiredAngle();

				bool isOnTarget = MoveBarrelToAngle(desiredAngleInRadians);

				_timeSinceLastFireInS += Time.deltaTime;

				if (isOnTarget && _timeSinceLastFireInS >= turretStats.FireIntervalInS)
				{
					Fire(desiredAngleInRadians);
					_timeSinceLastFireInS = 0;
				}
			}
		}
		
		/// <summary>
		/// Assumes:
		/// 1. Shells are not affected by gravity
		/// 2. Targets do not move
		/// </summary>
		protected virtual float FindDesiredAngle()
		{
			// FELIX  This is wrong :P  Find angle for direct fire from shell spawner to target
			float desiredAngle = Target.transform.rotation.z;
			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"TurretBarrelController.FindDesiredAngle() {desiredAngle}");
			return desiredAngle;
		}

		private bool MoveBarrelToAngle(float desiredAngleInRadians)
		{
			float currentAngleInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
			bool isCorrectAngle = Math.Abs(currentAngleInRadians - desiredAngleInRadians) < ROTATION_EQUALITY_MARGIN_IN_RADIANS;
			
			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"Update():  currentAngleInRadians: {currentAngleInRadians}  DesiredAngleInRadians: {desiredAngleInRadians}  isCorrectAngle: {isCorrectAngle}");
			
			if (!isCorrectAngle)
			{
				float directionMultiplier = transform.rotation.z > desiredAngleInRadians ? -1 : 1;
				Vector3 rotationIncrement = Vector3.forward * Time.deltaTime * ROTATE_SPEED_IN_DEGREES_PER_S * directionMultiplier;
				transform.Rotate(rotationIncrement);
			}

			return isCorrectAngle;
		}
		
		private void Fire(float angleInRadians)
		{
			Direction fireDirection = Target.transform.position.x > transform.position.x ? Direction.Right : Direction.Left;
			shellSpawner.SpawnShell(angleInRadians, fireDirection);
		}
	}
}
