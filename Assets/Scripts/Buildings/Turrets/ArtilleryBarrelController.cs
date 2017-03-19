using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings.Turrets
{
	// FELIX  Move to own file
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
			float distanceInM = Math.Abs(transform.position.x - _targetObject.transform.position.x);
			if (distanceInM > _maxRange)
			{
				throw new InvalidProgramException();
			}
			return (float) (0.5 * Math.Asin(Constants.GRAVITY * distanceInM / (_shellVelocityInMPerS * _shellVelocityInMPerS)));
		}
	}
}
