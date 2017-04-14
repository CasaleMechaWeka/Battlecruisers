using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	/// <summary>
	/// Artillery barrel wrapper controller.
	/// FELIX  Lead moving targets!
	/// FELIX  Take accuracy into consideration
	/// </summary>
	public class ArtilleryBarrelController : TurretBarrelController
	{
		/// <summary>
		/// Assumes no y axis difference in source and target
		/// </summary>
		protected override float FindDesiredAngle()
		{
			float distanceInM = Math.Abs(transform.position.x - Target.transform.position.x);
			if (distanceInM > _maxRange)
			{
				throw new InvalidProgramException();
			}
			float angleInRadians = (float) (0.5 * Math.Asin(Constants.GRAVITY * distanceInM / (turretStats.bulletVelocityInMPerS * turretStats.bulletVelocityInMPerS)));
			float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"ArtilleryBarrelController.FindDesiredAngle() {angleInRadians} radians  {angleInDegrees}*");

			return angleInDegrees;
		}
	}
}
