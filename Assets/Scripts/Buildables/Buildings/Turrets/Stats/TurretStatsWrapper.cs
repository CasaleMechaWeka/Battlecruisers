using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class TurretStatsWrapper : ITurretStatsWrapper
    {
        private ITurretStats _turretStats;
        public ITurretStats TurretStats 
        { 
            private get { return _turretStats; } 
            set
            {
                Assert.IsNotNull(value);
                _turretStats = value;
            }
        }

        public float Accuracy { get { return TurretStats.Accuracy; } }
        public float TurretRotateSpeedInDegrees { get { return TurretStats.TurretRotateSpeedInDegrees; } }
        public bool IsInBurst { get { return TurretStats.IsInBurst; } }
        public float FireRatePerS { get { return TurretStats.FireRatePerS; } }
        public float RangeInM { get { return TurretStats.RangeInM; } }
        public float MinRangeInM { get { return TurretStats.MinRangeInM; } }
        public float MeanFireRatePerS { get { return TurretStats.MeanFireRatePerS; } }
        public ReadOnlyCollection<TargetType> AttackCapabilities { get { return TurretStats.AttackCapabilities; } }
        public float DurationInS { get { return TurretStats.DurationInS; } }

        public TurretStatsWrapper(ITurretStats turretStats)
        {
            TurretStats = turretStats;
        }

        public void MoveToNextDuration()
        {
            _turretStats.MoveToNextDuration();
        }
    }
}
