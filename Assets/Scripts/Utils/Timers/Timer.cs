using System;
using UnityEngine;

namespace BattleCruisers.Utils.Timers
{
    public class Timer : ITimer
    {
        private float _timeElapsedInS;
        private int _timeElapsedInFullS;
        private bool _timerStarted;

        public event EventHandler<TimerEventArgs> OnSecondPassed;

        public bool IsRunning { get { return _timerStarted; } }
		
        public Timer()
        {
            _timerStarted = false;
			_timeElapsedInS = 0;
			_timeElapsedInFullS = 0;
        }

        public void Start()
        {
            _timerStarted = true;
        }

        public void Stop()
        {
            _timerStarted = false;
        }

        public void OnUpdate(float timeInS)
        {
            if (_timerStarted)
            {
                _timeElapsedInS += timeInS;

                // Emit event if a full second has passed
                int timeElapsedInFullS = Mathf.FloorToInt(_timeElapsedInS);

                if (timeElapsedInFullS != _timeElapsedInFullS)
                {
                    _timeElapsedInFullS = timeElapsedInFullS;

                    OnSecondPassed?.Invoke(this, new TimerEventArgs(_timeElapsedInFullS));
                }
            }
        }
    }
}
