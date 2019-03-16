using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class BasicTurretStats : MonoBehaviour, IBasicTurretStats
    {
        public float fireRatePerS;
        public float FireRatePerS => fireRatePerS;
        public virtual float MeanFireRatePerS => FireRatePerS;

        public float rangeInM;
        public float RangeInM => rangeInM;

        public float minRangeInM;
        public float MinRangeInM => minRangeInM;

        public virtual float DurationInS => 1 / FireRatePerS;

        public List<TargetType> attackCapabilities;
        public ReadOnlyCollection<TargetType> AttackCapabilities { get; private set; }

        public virtual void Initialise()
		{
			Assert.IsTrue(FireRatePerS > 0);
			Assert.IsTrue(RangeInM > 0);
            Assert.IsTrue(RangeInM > MinRangeInM);
            Assert.IsTrue(attackCapabilities.Count > 0);

            AttackCapabilities = new ReadOnlyCollection<TargetType>(attackCapabilities);
		}

        public virtual void MoveToNextDuration()
        {
            // Empty
        }
    }
}

