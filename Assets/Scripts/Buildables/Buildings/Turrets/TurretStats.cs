using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class TurretStats : MonoBehaviour
	{
		public float fireRatePerS;
		public float accuracy;
		public float damage;
		public float bulletVelocityInMPerS;
		public bool ignoreGravity;
		public float rangeInM;
		public float turretRotateSpeedInDegrees;
		public ShellController shellPrefab;

		public float DamagePerS { get { return damage * fireRatePerS; } }
		public float FireIntervalInS { get { return 1 / fireRatePerS; } }

		void Awake()
		{
			Assert.IsTrue(fireRatePerS > 0);
			Assert.IsTrue(accuracy >= 0 && accuracy <= 1);
			Assert.IsTrue(damage > 0);
			Assert.IsTrue(bulletVelocityInMPerS > 0);
			Assert.IsTrue(rangeInM > 0);
			Assert.IsTrue(turretRotateSpeedInDegrees > 0);
			Assert.IsNotNull(shellPrefab);
		}
	}
}