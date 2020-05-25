using BattleCruisers.Utils.Timers;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Debugging
{
    public class CheaterButtonsPanelToggler : MonoBehaviour
    {
        private IDebouncer _debouncer;

        public float debounceTimeInS = 1;
        public int numOfContactPoints = 4;
        public GameObject cheaterButtonsPanel;

        void Start()
        {
            Assert.IsNotNull(cheaterButtonsPanel);
            _debouncer = new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS);
        }

        void Update()
        {
            if (Input.touchCount == numOfContactPoints)
            {
                _debouncer.Debounce(ToggleCheatersUI);
            }
        }

        private void ToggleCheatersUI()
        {
            cheaterButtonsPanel.SetActive(!cheaterButtonsPanel.activeSelf);
        }
    }
}