using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class BasicTurretStats : MonoBehaviour, IDurationProvider, IBoostable
	{
		public float fireRatePerS;
		public float damage;
		public float rangeInM;

        private const float DEFAULT_FIRE_RATE_MULTIPLIER = 1;

        protected float EffectiveFireRatePerS { get { return BoostMultiplier * fireRatePerS; } }

        public virtual float DamagePerS { get { return damage * EffectiveFireRatePerS; } }
		public virtual float DurationInS { get { return 1 / EffectiveFireRatePerS; } }
        public float BoostMultiplier { get; set; }

        public virtual void Initialise()
		{
			Assert.IsTrue(fireRatePerS > 0);
			Assert.IsTrue(damage > 0);
			Assert.IsTrue(rangeInM > 0);

            BoostMultiplier = DEFAULT_FIRE_RATE_MULTIPLIER;
		}

        public virtual void MoveToNextDuration()
        {
            // Empty
        }
    }
}

