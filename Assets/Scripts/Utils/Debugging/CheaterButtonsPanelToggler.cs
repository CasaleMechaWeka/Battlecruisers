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

        [Header("Battle Sequencer Message Display")]
        [Tooltip("Optional message display component. Should be on a child canvas within the cheater panel. Will show/hide with the panel.")]
        public BattleSceneMessageDisplay messageDisplay;

        void Start()
        {
            Assert.IsNotNull(cheaterButtonsPanel);

#if !ENABLE_CHEATS
            Destroy(cheaterButtonsPanel);
            Destroy(gameObject);
            return;
#endif

            _debouncer = new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS);

            // Auto-find message display if not assigned
            if (messageDisplay == null)
            {
                messageDisplay = cheaterButtonsPanel.GetComponentInChildren<BattleSceneMessageDisplay>(includeInactive: true);
            }

            // Connect message display to BattleSequencer if found
            if (messageDisplay != null)
            {
                ConnectMessageDisplayToBattleSequencer();
            }
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

            // Show/hide message display along with panel
            if (messageDisplay != null)
            {
                messageDisplay.gameObject.SetActive(isActive);
                
                // Refresh display when showing panel to display any queued messages
                if (isActive)
                {
                    // Force update to show any queued messages
                    messageDisplay.UpdateDisplay();
                }
            }
        }

        /// <summary>
        /// Finds BattleSequencer in the scene and connects the message display to it
        /// </summary>
        private void ConnectMessageDisplayToBattleSequencer()
        {
            // Try to find BattleSequencer on the AI cruiser (embedded approach)
            BattleSequencer sequencer = FindObjectOfType<BattleSequencer>();
            
            if (sequencer != null)
            {
                sequencer.messageDisplay = messageDisplay;
                Debug.Log("[CheaterButtonsPanelToggler] Connected message display to BattleSequencer");
            }
            else
            {
                Debug.LogWarning("[CheaterButtonsPanelToggler] BattleSequencer not found. Message display will not receive messages.");
            }
        }
    }
}