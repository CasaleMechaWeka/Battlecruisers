using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        // FELIX  Remove.  Local and global boost should be handled in same class (BoostedBasicTurretStats :) )
        public float BoostMultiplier { get; set; }

        public List<TargetType> attackCapabilities;
        public ReadOnlyCollection<TargetType> AttackCapabilities { get; private set; }

        public virtual void Initialise()
		{
			Assert.IsTrue(FireRatePerS > 0);
			Assert.IsTrue(RangeInM > 0);
            Assert.IsTrue(RangeInM > MinRangeInM);
            Assert.IsTrue(attackCapabilities.Count > 0);

            BoostMultiplier = DEFAULT_FIRE_RATE_MULTIPLIER;

            AttackCapabilities = new ReadOnlyCollection<TargetType>(attackCapabilities);
		}

        public virtual void MoveToNextDuration()
        {
            // Empty
        }
    }
}

