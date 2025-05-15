using BattleCruisers.Data;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class SpeedPanelController : MonoBehaviour
    {
        public GameSpeedButton slowMotion, normalSpeed, fastForward, pause;

        public SpeedComponents Initialise(SingleSoundPlayer soundPlayer, IBroadcastingFilter shouldBeEnabledFilter)
        {
            Helper.AssertIsNotNull(soundPlayer, shouldBeEnabledFilter);
            Helper.AssertIsNotNull(slowMotion, normalSpeed, fastForward);

            List<GameSpeedButton> speedButtons = new List<GameSpeedButton>()
            {
                slowMotion,
                normalSpeed,
                fastForward,
                pause
            };

            foreach (GameSpeedButton speedButton in speedButtons)
            {
                speedButton.Initialise(soundPlayer, shouldBeEnabledFilter, TimeBC.Instance);
            }

            if (!DataProvider.GameModel.PremiumEdition || ApplicationModel.Mode != GameMode.Skirmish)
                pause.gameObject.SetActive(false);

            IToggleButtonGroup speedButtonGroup = new ToggleButtonGroup(speedButtons.ToList<IToggleButton>(), normalSpeed);

            Highlightable speedButtonPanel = GetComponent<Highlightable>();
            Assert.IsNotNull(speedButtonPanel);
            speedButtonPanel.Initialise();

            return
                new SpeedComponents(
                    speedButtonPanel,
                    speedButtonGroup,
                    slowMotion,
                    normalSpeed,
                    fastForward,
                    pause
                );
        }
    }
}