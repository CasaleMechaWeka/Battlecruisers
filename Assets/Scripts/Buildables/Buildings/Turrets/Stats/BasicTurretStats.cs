using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class BasicTurretStats : MonoBehaviour, IDurationProvider
	{
		public float fireRatePerS;
		public float damage;
		public float rangeInM;

		public virtual float DamagePerS { get { return damage * fireRatePerS; } }
		public virtual float NextDurationInS { get { return 1 / fireRatePerS; } }

		public virtual void Initialise()
		{
			Assert.IsTrue(fireRatePerS > 0);
			Assert.IsTrue(damage > 0);
			Assert.IsTrue(rangeInM > 0);
		}
	}
}

