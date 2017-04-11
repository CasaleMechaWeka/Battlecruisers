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
		public float DamagePerS { get { return damage * fireRatePerS; } }
		public float FireIntervalInS { get { return 1 / fireRatePerS; } }

		void Awake()
		{
			Assert.IsTrue(
				fireRatePerS > 0
				&& accuracy >= 0
				&& accuracy <= 1
				&& damage > 0
				&& bulletVelocityInMPerS > 0);
		}
	}
}