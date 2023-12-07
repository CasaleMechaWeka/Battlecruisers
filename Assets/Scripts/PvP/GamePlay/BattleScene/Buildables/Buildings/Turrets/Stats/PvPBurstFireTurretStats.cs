using BattleCruisers.UI.ScreensScene.ProfileScreen;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    /// <summary>
    /// For example, if burst size = 3:
    /// 
    /// InBurst (T = true / F = false):         F  T  T    F  T  T    F
    /// Turret fires (MoveToNextDuration()):     *  *  *    *  *  *
    /// Duration (S = short / L = long):        L  S  S    L  S  S    L
    /// </summary>
    public class PvPBurstFireTurretStats : PvPTurretStats
    {
        private float _shortDurationInS;

        public float burstFireRatePerS;

        private const int MIN_BURST_SIZE = 1;

        public int burstSize;
        public override int BurstSize => burstSize;

        private int _queryIndex;
        private int QueryIndex
        {
            get { return _queryIndex; }
            set
            {
                _queryIndex = value;

                if (_queryIndex % burstSize == 0)
                {
                    _queryIndex = 0;
                }
            }
        }

        public override float DurationInS
        {
            get
            {
                return QueryIndex == 0 ? LongDurationInS : _shortDurationInS;
            }
        }

        private float LongDurationInS => 1 / FireRatePerS;

        public override bool IsInBurst
        {
            get
            {
                return QueryIndex != 0;
            }
        }

        private float _meanFireRatePerS;
        public override float MeanFireRatePerS => _meanFireRatePerS;

        public override void Initialise()
        {
            base.Initialise();

            Assert.IsTrue(burstSize >= MIN_BURST_SIZE);
            Assert.IsTrue(burstFireRatePerS > 0);

            _shortDurationInS = 1 / burstFireRatePerS;

            float cycleTime = (1 / FireRatePerS) + (burstSize - 1) * (1 / burstFireRatePerS);
            _meanFireRatePerS = burstSize / cycleTime;

            QueryIndex = 0;
        }

        public override void MoveToNextDuration()
        {
            QueryIndex++;
        }

        public override void ApplyVariantStats(StatVariant statVariant)
        {
            if (!isAppliedVariant)
            {
                base.ApplyVariantStats(statVariant);
                burstFireRatePerS += statVariant.burst_fire_rate;
                burstSize += statVariant.burst_size;

                burstFireRatePerS = burstFireRatePerS <= 0 ? 0.1f : burstFireRatePerS;
                burstSize = burstSize < MIN_BURST_SIZE ? MIN_BURST_SIZE : burstSize;
            }
        }
    }
}
