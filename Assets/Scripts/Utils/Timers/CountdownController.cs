using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Timers
{
    public class CountdownController : MonoBehaviour
    {
        private TextMesh _countdownText;
        private ICountdown _countdown;
		private Action _onCompletion;

        public int durationInS;

        public bool IsInProgress => gameObject.activeSelf;

        public void Initialise()
        {
            _countdownText = GetComponent<TextMesh>();
            Assert.IsNotNull(_countdownText);

            Assert.IsTrue(durationInS > 0);

            _countdown = new Countdown();
            _countdown.OnSecondPassed += _countdown_OnSecondPassed;

            gameObject.SetActive(false);
        }

        private void _countdown_OnSecondPassed(object sender, CountdownEventArgs e)
        {
            _countdownText.text = e.SecondsRemaining.ToString();
        }
		
        public void Begin(Action onCompletion)
        {
            Assert.IsNotNull(onCompletion);
            _onCompletion = onCompletion;

            _countdownText.text = durationInS.ToString();
            gameObject.SetActive(true);

            _countdown.Start(durationInS, OnCompletion);
        }

        private void OnCompletion()
        {
            _onCompletion.Invoke();
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            _countdown.Cancel();
            gameObject.SetActive(false);
        }

        void Update()
        {
            _countdown.OnUpdate(Time.deltaTime);
        }
    }
}
