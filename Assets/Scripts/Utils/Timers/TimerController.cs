using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Timers
{
    public class TimerController : MonoBehaviour
    {
        private string _prefix, _suffix;
        private TextMesh _timerText;
        private ITimer _timer;
        private ITime _time;

        private int TimeElapsed
        {
            set
            {
                _timerText.text = _prefix + value + _suffix;
            }
        }

        public bool IsRunning => _timer.IsRunning;

        public void Initialise(string prefix, string suffix)
        {
            Helper.AssertIsNotNull(prefix, suffix);

            gameObject.SetActive(true);

            _prefix = prefix;
            _suffix = suffix;

            _timerText = GetComponent<TextMesh>();
            Assert.IsNotNull(_timerText);
            TimeElapsed = 0;

            _timer = new Timer();
            _timer.OnSecondPassed += _timer_OnSecondPassed;

            _time = TimeBC.Instance;
        }

        private void _timer_OnSecondPassed(object sender, TimerEventArgs e)
        {
            TimeElapsed = e.SecondsElapsed;
        }

        public void Begin()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        void Update()
        {
            _timer.OnUpdate(_time.DeltaTime);
        }
    }
}
