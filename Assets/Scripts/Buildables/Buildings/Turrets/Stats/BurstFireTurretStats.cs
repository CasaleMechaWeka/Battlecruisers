using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    /// <summary>
    /// For example, if burst size = 3:
    /// 
    /// Duration (S = short / L = long):    S S L   S S L
    /// InBurst (T = true / F = false):     F T T   F T T
    /// </summary>
    public class BurstFireTurretStats : TurretStats
    {
        private float _shortDurationInS;

        public float burstFireRatePerS;

        private const int MIN_BURST_SIZE = 2;

        public int burstSize;
        public override int BurstSize { get { return burstSize; } }

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
                return QueryIndex == burstSize - 1 ? LongDurationInS : _shortDurationInS;
            }
        }

        private float LongDurationInS { get { return 1 / FireRatePerS; } }

        public override bool IsInBurst 
        { 
            get 
            {
                return QueryIndex != 0;
            }
        }

        private float _meanFireRatePerS;
        public override float MeanFireRatePerS { get { return _meanFireRatePerS; } }

		public override void Initialise()
		{
			base.Initialise();

			Assert.IsTrue(burstSize >= MIN_BURST_SIZE);
			Assert.IsTrue(burstFireRatePerS > 0);

			_shortDurationInS = 1 / burstFireRatePerS;

            float cycleTime = (1 / FireRatePerS) + burstSize * (1 / burstFireRatePerS);
            _meanFireRatePerS = burstSize / cycleTime;

            QueryIndex = 0;
		}

        public override void MoveToNextDuration()
        {
            QueryIndex++;
		}
	}
}
