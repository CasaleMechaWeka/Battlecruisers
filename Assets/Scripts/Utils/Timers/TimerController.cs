using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Timers
{
    public class TimerController : MonoBehaviour
    {
        private TextMesh _timerText;
        private ITimer _timer;

        public void Initialise()
        {
            _timerText = GetComponent<TextMesh>();
            Assert.IsNotNull(_timerText);
            _timerText.text = "0";

            _timer = new Timer();
            _timer.OnSecondPassed += _timer_OnSecondPassed;
        }

        private void _timer_OnSecondPassed(object sender, TimerEventArgs e)
        {
            _timerText.text = e.SecondsElapsed.ToString();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        void Update()
        {
            _timer.OnUpdate(Time.deltaTime);
        }
    }
}
