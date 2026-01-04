using BattleCruisers.Utils.Timers;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Scenes.BattleScene;
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

#if !ENABLE_CHEATS
            Destroy(cheaterButtonsPanel);
            Destroy(gameObject);
            return;
#endif

            _debouncer = new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS);

        }

        void Update()
        {
            if (Input.touchCount == numOfContactPoints
                || Input.GetKeyUp(KeyCode.F3))
            {
                _debouncer.Debounce(ToggleCheatersUI);
            }
        }

        private void ToggleCheatersUI()
        {
            bool isActive = !cheaterButtonsPanel.activeSelf;
            cheaterButtonsPanel.SetActive(isActive);
        }
    }
}