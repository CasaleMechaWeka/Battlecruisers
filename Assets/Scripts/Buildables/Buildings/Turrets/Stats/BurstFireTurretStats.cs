using BattleCruisers.Buildables.Buildings.Turrets.Stats.States;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class BurstFireTurretStats : TurretStats
    {
        private IBurstFireState _currentState;

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

        public override float DurationInS { get { return _currentState.DurationInS; } }

        public override bool IsInBurst { get { return _currentState.IsInBurst; } }

		public override void Initialise()
		{
			base.Initialise();

			Assert.IsTrue(burstSize >= MIN_BURST_SIZE);
			Assert.IsTrue(burstFireRatePerS > 0);

            BurstFireState shortDurationState = new BurstFireState();
            BurstFireState longDurationState = new BurstFireState();

            shortDurationState.Initialise(
                otherState: longDurationState,
                durationInS: 1 / burstFireRatePerS,
                numOfQueriesBeforeSwitch: burstSize - 1);

            longDurationState.Initialise(
                otherState: shortDurationState,
                durationInS: 1 / fireRatePerS,
                numOfQueriesBeforeSwitch: 1);

            _currentState = shortDurationState;
		}

        public override void MoveToNextDuration()
        {
			_currentState = _currentState.NextState;
		}
	}
}
