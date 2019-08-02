using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.Filters;
using System.Linq;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class SpeedPanelController : MonoBehaviour
    {
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private ToggleButtonGroup _speedButtonGroup;
#pragma warning restore CS0414  // Variable is assigned but never used

        private const int EXPECTED_NUM_OF_BUTTONS = 2;  // Slow motion, fast forward

        public IMaskHighlightable Initialise(IBroadcastingFilter shouldBeEnabledFilter)
        {
            Assert.IsNotNull(shouldBeEnabledFilter);

            GameSpeedButton[] speedButtons = GetComponentsInChildren<GameSpeedButton>();
            Assert.AreEqual(EXPECTED_NUM_OF_BUTTONS, speedButtons.Length);
            ITime time = new TimeBC();

            foreach (GameSpeedButton speedButton in speedButtons)
            {
                speedButton.Initialise(shouldBeEnabledFilter, time);
            }

            _speedButtonGroup = new ToggleButtonGroup(speedButtons.ToList<IToggleButton>());

            MaskHighlightable speedButtonPanel = GetComponent<MaskHighlightable>();
            Assert.IsNotNull(speedButtonPanel);
            speedButtonPanel.Initialise();
            return speedButtonPanel;
        }
    }
}