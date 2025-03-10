using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Timers
{
    public class Countdown : ICountdown
    {
        private float _timeElapsedInS;
		private int _timeElapsedInFullS;
        private int _durationInS;
        private Action _onCompletion;
        private bool _countdownInProgress;

        public event EventHandler<CountdownEventArgs> OnSecondPassed;

        public void Start(int durationInS, Action onCompletion)
        {
            Assert.IsTrue(durationInS > 0);
			Assert.IsNotNull(onCompletion);

            _durationInS = durationInS;
			_onCompletion = onCompletion;
            _timeElapsedInS = 0;
            _timeElapsedInFullS = 0;

            _countdownInProgress = true;
        }

        public void Cancel()
        {
            _countdownInProgress = false;
        }

        public void OnUpdate(float timeInS)
        {
            Assert.IsTrue(_countdownInProgress);

            _timeElapsedInS += timeInS;

            // Emit event if a full second has passed
            int timeElapsedInFullS = Mathf.FloorToInt(_timeElapsedInS);

            if (timeElapsedInFullS != _timeElapsedInFullS)
            {
                _timeElapsedInFullS = timeElapsedInFullS;

                int secondsRemaining = _durationInS - _timeElapsedInFullS;
                OnSecondPassed?.Invoke(this, new CountdownEventArgs(secondsRemaining));
            }

            // Check if the countdown is over
            if (_timeElapsedInS >= _durationInS)
            {
                _onCompletion.Invoke();
                _countdownInProgress = false;
            }
        }
    }
}
