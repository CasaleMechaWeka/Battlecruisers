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
        private float _longDurationInS;

        public int burstSize;
        public float burstFireRatePerS;

        private const int MIN_BURST_SIZE = 3;

        public override float DamagePerS
        {
            get
            {
                float cycleDamage = burstSize * damage;
                float cycleTime = (1 / fireRatePerS) + burstSize * (1 / burstFireRatePerS);
                return cycleDamage / cycleTime;
            }
        }

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
                return QueryIndex == burstSize - 1 ? _longDurationInS : _shortDurationInS;
            }
        }

        public override bool IsInBurst 
        { 
            get 
            {
                return QueryIndex != 0;
            }
        }

		public override void Initialise()
		{
			base.Initialise();

			Assert.IsTrue(burstSize >= MIN_BURST_SIZE);
			Assert.IsTrue(burstFireRatePerS > 0);

			_shortDurationInS = 1 / burstFireRatePerS;
            _longDurationInS = 1 / fireRatePerS;

            QueryIndex = 0;
		}

        public override void MoveToNextDuration()
        {
            QueryIndex++;
		}
	}
}
