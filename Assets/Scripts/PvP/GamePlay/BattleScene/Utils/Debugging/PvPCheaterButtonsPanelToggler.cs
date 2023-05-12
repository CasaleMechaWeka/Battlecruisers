using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Debugging
{
    public class PvPCheaterButtonsPanelToggler : MonoBehaviour
    {
        private IPvPDebouncer _debouncer;

        public float debounceTimeInS = 1;
        public int numOfContactPoints = 4;
        public GameObject cheaterButtonsPanel;

        void Start()
        {
            Assert.IsNotNull(cheaterButtonsPanel);

#if !ENABLE_CHEATS
            Destroy(cheaterButtonsPanel);
            Destroy(gameObject);
#endif

            _debouncer = new PvPDebouncer(PvPTimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS);
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
            cheaterButtonsPanel.SetActive(!cheaterButtonsPanel.activeSelf);
        }
    }
}