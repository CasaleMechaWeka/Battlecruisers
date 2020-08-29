using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityCommon.PlatformAbstractions.Time;
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

        public GameSpeedButton slowMotion, normalSpeed, fastForward;

        public IHighlightable Initialise(ISingleSoundPlayer soundPlayer, IBroadcastingFilter shouldBeEnabledFilter)
        {
            Helper.AssertIsNotNull(soundPlayer, shouldBeEnabledFilter);
            Helper.AssertIsNotNull(slowMotion, normalSpeed, fastForward);

            List<GameSpeedButton> speedButtons = new List<GameSpeedButton>()
            {
                slowMotion,
                normalSpeed,
                fastForward
            };

            foreach (GameSpeedButton speedButton in speedButtons)
            {
                speedButton.Initialise(soundPlayer, shouldBeEnabledFilter, TimeBC.Instance);
            }

            _speedButtonGroup = new ToggleButtonGroup(speedButtons.ToList<IToggleButton>(), normalSpeed);

            Highlightable speedButtonPanel = GetComponent<Highlightable>();
            Assert.IsNotNull(speedButtonPanel);
            speedButtonPanel.Initialise();
            return speedButtonPanel;
        }
    }
}