using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class BasicTurretStats : MonoBehaviour, IBasicTurretStats, IDurationProvider, IBoostable
	{
		private const float DEFAULT_FIRE_RATE_MULTIPLIER = 1;

        public float fireRatePerS;
        public float FireRatePerS { get { return fireRatePerS; } }
        public virtual float MeanFireRatePerS { get { return FireRatePerS; } }

        public float rangeInM;
        public float RangeInM { get { return rangeInM; } }
        
        public float minRangeInM;
        public float MinRangeInM { get { return minRangeInM; } }

        protected float EffectiveFireRatePerS { get { return BoostMultiplier * fireRatePerS; } }
        public virtual float DurationInS { get { return 1 / EffectiveFireRatePerS; } }
        public float BoostMultiplier { get; set; }

        public virtual void Initialise()
		{
			Assert.IsTrue(FireRatePerS > 0);
			Assert.IsTrue(RangeInM > 0);
            Assert.IsTrue(RangeInM > MinRangeInM);

            BoostMultiplier = DEFAULT_FIRE_RATE_MULTIPLIER;
		}

        public virtual void MoveToNextDuration()
        {
            // Empty
        }
    }
}

