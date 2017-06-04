using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class BasicTurretStats : MonoBehaviour, IFireIntervalProvider
	{
		public float fireRatePerS;
		public float damage;
		public float rangeInM;

		public virtual float DamagePerS { get { return damage * fireRatePerS; } }
		public virtual float NextFireIntervalInS { get { return 1 / fireRatePerS; } }

		public virtual void Initialise()
		{
			Assert.IsTrue(fireRatePerS > 0);
			Assert.IsTrue(damage > 0);
			Assert.IsTrue(rangeInM > 0);
		}
	}
}

