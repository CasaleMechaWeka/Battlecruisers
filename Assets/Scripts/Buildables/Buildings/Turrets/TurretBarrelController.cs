using BattleCruisers.Buildables.Units;
using BattleCruisers.Projectiles;
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
		private Faction _faction;
		private float _timeSinceLastFireInS;
		private ShellStats _shellStats;

		// FELIX  Allow to vary depending on artillery?  Perhaps also part of TurretStats?
		public ShellController shellPrefab;
		public ShellSpawnerController shellSpawner;
		public TurretStats turretStats;
		public AngleCalculator angleCalculator;

		public GameObject Target { get; set; }
		private bool IsSourceMirrored { get { return transform.rotation.eulerAngles.y == 180; } }

		private const float ROTATION_EQUALITY_MARGIN_IN_DEGREES = 1;

		void Awake()
		{
			_timeSinceLastFireInS = float.MaxValue;
			_shellStats = new ShellStats(shellPrefab, turretStats.damage, turretStats.ignoreGravity, turretStats.bulletVelocityInMPerS);
			shellSpawner.Initialise(_faction, _shellStats);
		}

		public void Initialise(Faction faction)
		{
			_faction = faction;
		}

		void Update()
		{
			if (Target != null)
			{
				Vector2 source = new Vector2(transform.position.x, transform.position.y);
				Vector2 target = new Vector2(Target.transform.position.x, Target.transform.position.y);
				
				float desiredAngleInDegrees = angleCalculator.FindDesiredAngle(source, target, IsSourceMirrored, turretStats.bulletVelocityInMPerS);

				bool isOnTarget = MoveBarrelToAngle(desiredAngleInDegrees);

				_timeSinceLastFireInS += Time.deltaTime;

				if (isOnTarget && _timeSinceLastFireInS >= turretStats.FireIntervalInS)
				{
					Fire(desiredAngleInDegrees);
					_timeSinceLastFireInS = 0;
				}
			}
		}

		private bool MoveBarrelToAngle(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = transform.rotation.eulerAngles.z;
			float differenceInDegrees = Math.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			bool isCorrectAngle = differenceInDegrees < ROTATION_EQUALITY_MARGIN_IN_DEGREES;
			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"MoveBarrelToAngle():  currentAngleInDegrees: {currentAngleInDegrees}  desiredAngleInDegrees: {desiredAngleInDegrees}  isCorrectAngle: {isCorrectAngle}");
			
			if (!isCorrectAngle)
			{
				float directionMultiplier = angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, desiredAngleInDegrees);
				Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"directionMultiplier: {directionMultiplier}");

				float rotationIncrement = Time.deltaTime * turretStats.turretRotateSpeedInDegrees;
				if (rotationIncrement > differenceInDegrees)
				{
					rotationIncrement = differenceInDegrees;
				}
				Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement * directionMultiplier;
				Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"rotationIncrement: {rotationIncrement}");

				transform.Rotate(rotationIncrementVector);
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
