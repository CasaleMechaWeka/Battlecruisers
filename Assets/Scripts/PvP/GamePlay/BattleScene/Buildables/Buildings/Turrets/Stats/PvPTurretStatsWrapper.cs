using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPTurretStatsWrapper : IPvPTurretStatsWrapper
    {
        private IPvPTurretStats _turretStats;
        public IPvPTurretStats pvpTurretStats
        {
            private get { return _turretStats; }
            set
            {
                Assert.IsNotNull(value);
                _turretStats = value;
            }
        }

        public float Accuracy => pvpTurretStats.Accuracy;
        public float TurretRotateSpeedInDegrees => pvpTurretStats.TurretRotateSpeedInDegrees;
        public bool IsInBurst => pvpTurretStats.IsInBurst;
        public float FireRatePerS => pvpTurretStats.FireRatePerS;
        public float RangeInM => pvpTurretStats.RangeInM;
        public float MinRangeInM => pvpTurretStats.MinRangeInM;
        public float MeanFireRatePerS => pvpTurretStats.MeanFireRatePerS;
        public ReadOnlyCollection<PvPTargetType> AttackCapabilities => pvpTurretStats.AttackCapabilities;
        public float DurationInS => pvpTurretStats.DurationInS;
        public int BurstSize => pvpTurretStats.BurstSize;

        public PvPTurretStatsWrapper(IPvPTurretStats turretStats)
        {
            pvpTurretStats = turretStats;
        }

        public void MoveToNextDuration()
        {
            _turretStats.MoveToNextDuration();
        }
    }
}
