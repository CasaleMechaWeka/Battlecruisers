using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPBasicTurretStats : MonoBehaviour, IPvPBasicTurretStats
    {
        public float fireRatePerS;
        public float FireRatePerS => fireRatePerS;
        public virtual float MeanFireRatePerS => FireRatePerS;

        public float rangeInM;
        public float RangeInM => rangeInM;

        public float minRangeInM;
        public float MinRangeInM => minRangeInM;

        public virtual float DurationInS => 1 / FireRatePerS;

        public List<PvPTargetType> attackCapabilities;
        public ReadOnlyCollection<PvPTargetType> AttackCapabilities { get; private set; }

        public virtual void Initialise()
        {
            Assert.IsTrue(FireRatePerS > 0);
            Assert.IsTrue(RangeInM > 0);
            Assert.IsTrue(RangeInM > MinRangeInM);
            Assert.IsTrue(attackCapabilities.Count > 0);

            AttackCapabilities = new ReadOnlyCollection<PvPTargetType>(attackCapabilities);
        }

        public virtual void MoveToNextDuration()
        {
            // Empty
        }
    }
}

