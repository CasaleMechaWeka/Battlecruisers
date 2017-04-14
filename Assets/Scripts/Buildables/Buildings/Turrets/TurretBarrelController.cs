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
		private const float ROTATION_EQUALITY_MARGIN_IN_DEGREES = 1;

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
			float xDiff = transform.position.x - Target.transform.position.x;
			float yDiff = transform.position.y - Target.transform.position.y;
			float angleInRadians = (float) Math.Atan(yDiff / xDiff);
			float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"FindDesiredAngle() {angleInRadians} radians  {angleInDegrees}*");

			return angleInDegrees;
		}

		private bool MoveBarrelToAngle(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = transform.rotation.eulerAngles.z;
			bool isCorrectAngle = Math.Abs(currentAngleInDegrees - desiredAngleInDegrees) < ROTATION_EQUALITY_MARGIN_IN_DEGREES;
			
			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"MoveBarrelToAngle():  currentAngleInDegrees: {currentAngleInDegrees}  desiredAngleInDegrees: {desiredAngleInDegrees}  isCorrectAngle: {isCorrectAngle}");
			
			if (!isCorrectAngle)
			{
				float directionMultiplier = transform.rotation.eulerAngles.z > desiredAngleInDegrees ? -1 : 1;
				Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"directionMultiplier: {directionMultiplier}");

				Vector3 rotationIncrement = Vector3.forward * Time.deltaTime * ROTATE_SPEED_IN_DEGREES_PER_S * directionMultiplier;
				Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"rotationIncrement: {rotationIncrement}");

				transform.Rotate(rotationIncrement);
			}

			return isCorrectAngle;
		}
		
		private void Fire(float angleInDegrees)
		{
			Direction fireDirection = Target.transform.position.x > transform.position.x ? Direction.Right : Direction.Left;
			shellSpawner.SpawnShell(angleInDegrees, fireDirection);
		}
	}
}
