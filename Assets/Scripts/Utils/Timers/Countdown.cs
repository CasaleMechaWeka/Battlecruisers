using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Timers
{
    /// <summary>
    /// Disables itself by setting the game object to inactive.  Hence, must have
    /// it's own game object, otherwise everything else on the game object will
    /// be disabled as well.
    /// </summary>
    public class Countdown : MonoBehaviour, ICountdown
    {
        private float _timeElapsedInS;
		private int _timeElapsedInFullS;
        private int _durationInS;
        private Action _onCompletion;

        public event EventHandler<CountdownEventArgs> OnSecondPassed;

        // Avoid conflict with MonoBehaviour.Start
        void ICountdown.Start(int durationInS, Action onCompletion)
        {
            Assert.IsTrue(durationInS > 0);
			Assert.IsNotNull(onCompletion);

            _durationInS = durationInS;
			_onCompletion = onCompletion;
            _timeElapsedInS = 0;
            _timeElapsedInFullS = 0;

            gameObject.SetActive(true);
        }

        public void Cancel()
        {
            gameObject.SetActive(false);
        }

        void Update()
        {
            _timeElapsedInS += Time.deltaTime;

            // Emit event if a full second has passed
            int timeElapsedInFullS = Mathf.FloorToInt(_timeElapsedInS);

            if (timeElapsedInFullS != _timeElapsedInFullS)
            {
                _timeElapsedInFullS = timeElapsedInFullS;

                if (OnSecondPassed != null)
                {
                    int secondsRemaining = _durationInS - _timeElapsedInFullS;
                    OnSecondPassed.Invoke(this, new CountdownEventArgs(secondsRemaining));
                }
            }

            // Check if the countdown is over
            if (_timeElapsedInS >= _durationInS)
            {
                _onCompletion.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
