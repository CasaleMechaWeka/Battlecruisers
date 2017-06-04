using BattleCruisers.Projectiles;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class TurretStats : BasicTurretStats
	{
		public float accuracy;
		public float bulletVelocityInMPerS;
		public bool ignoreGravity;
		public float turretRotateSpeedInDegrees;

		public virtual bool IsInBurst { get { return false; } }

		public override void Initialise()
		{
			Assert.IsTrue(accuracy >= 0 && accuracy <= 1);
			Assert.IsTrue(bulletVelocityInMPerS > 0);
			Assert.IsTrue(turretRotateSpeedInDegrees > 0);
		}
	}
}
