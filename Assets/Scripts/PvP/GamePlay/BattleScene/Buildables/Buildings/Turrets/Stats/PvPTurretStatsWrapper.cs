using System.Collections.ObjectModel;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPTurretStatsWrapper : ITurretStatsWrapper
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

        public float Accuracy => TurretStats.Accuracy;
        public float TurretRotateSpeedInDegrees => TurretStats.TurretRotateSpeedInDegrees;
        public bool IsInBurst => TurretStats.IsInBurst;
        public float FireRatePerS => TurretStats.FireRatePerS;
        public float RangeInM => TurretStats.RangeInM;
        public float MinRangeInM => TurretStats.MinRangeInM;
        public float MeanFireRatePerS => TurretStats.MeanFireRatePerS;
        public ReadOnlyCollection<TargetType> AttackCapabilities => TurretStats.AttackCapabilities;
        public float DurationInS => TurretStats.DurationInS;
        public int BurstSize => TurretStats.BurstSize;

        public PvPTurretStatsWrapper(ITurretStats turretStats)
        {
            TurretStats = turretStats;
        }

        public void MoveToNextDuration()
        {
            _turretStats.MoveToNextDuration();
        }
    }
}
