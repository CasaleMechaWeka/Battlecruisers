using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
		private bool IsSourceMirrored { get { return transform.rotation.eulerAngles.y == 180; } }

		// FELIX  Add this rotate speed to turret stats
		private const float ROTATE_SPEED_IN_DEGREES_PER_S = 90;
//		private const float ROTATE_SPEED_IN_DEGREES_PER_S = 25;
//		private const float ROTATE_SPEED_IN_DEGREES_PER_S = 5;
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
				Vector2 source = new Vector2(transform.position.x, transform.position.y);
				Vector2 target = new Vector2(Target.transform.position.x, Target.transform.position.y);
				
				float desiredAngleInDegrees = FindDesiredAngle(source, target, IsSourceMirrored);

				bool isOnTarget = MoveBarrelToAngle(desiredAngleInDegrees);

				_timeSinceLastFireInS += Time.deltaTime;

				if (isOnTarget && _timeSinceLastFireInS >= turretStats.FireIntervalInS)
				{
					Fire(desiredAngleInDegrees);
					_timeSinceLastFireInS = 0;
				}
			}
		}

		// FELIX  Unit test!
		/// <summary>
		/// Assumes:
		/// 1. Shells are not affected by gravity
		/// 2. Targets do not move
		/// </summary>
		protected virtual float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored)
		{
			Assert.AreNotEqual(source, target);

			float desiredAngleInDegrees;

			if (source.x == target.x)
			{
				// On same x-axis
				desiredAngleInDegrees = source.y < target.y ? 90 : -90;
			}
			else if (source.y == target.y)
			{
				// On same y-axis
				if (source.x < target.x)
				{
					desiredAngleInDegrees = isSourceMirrored ? 180 : 0;
				}
				else
				{
					desiredAngleInDegrees = isSourceMirrored ? 0 : 180;
				}
			}
			else
			{
				float xDiff = Math.Abs(source.x - target.x);
				float yDiff = Math.Abs(source.y - target.y);
				float angleInDegrees = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;

				// Different x and y axes, so need to calculate the angle
				if (source.x < target.y)
				{
					// Source is to left of target
					if (source.y < target.y)
					{
						// Source is below target
						desiredAngleInDegrees = isSourceMirrored ? 180 - angleInDegrees : angleInDegrees;
					}
					else
					{
						// Source is above target
						desiredAngleInDegrees = isSourceMirrored ? 180 + angleInDegrees : 360 - angleInDegrees;
					}
				}
				else
				{
					// Source is to right of target
					if (source.y < target.y)
					{
						// Source is below target
						desiredAngleInDegrees = isSourceMirrored ? angleInDegrees : 180 - angleInDegrees;
					}
					else
					{
						// Source is above target
						desiredAngleInDegrees = isSourceMirrored ? 360 - angleInDegrees : 180 + angleInDegrees;
					}
				}
			}

			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"FindDesiredAngle() {desiredAngleInDegrees}*");

			return desiredAngleInDegrees;
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
			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, "Fire()");

			shellSpawner.SpawnShell(angleInDegrees, IsSourceMirrored);
		}
	}
}
