using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public class PvPSpeedPanelController : MonoBehaviour
    {
        public PvPGameSpeedButton slowMotion, normalSpeed, fastForward, pause;

        public PvPSpeedComponents Initialise(ISingleSoundPlayer soundPlayer, IBroadcastingFilter shouldBeEnabledFilter)
        {
            PvPHelper.AssertIsNotNull(soundPlayer, shouldBeEnabledFilter);
            PvPHelper.AssertIsNotNull(slowMotion, normalSpeed, fastForward);

            List<PvPGameSpeedButton> speedButtons = new List<PvPGameSpeedButton>()
            {
                slowMotion,
                normalSpeed,
                fastForward,
                pause
            };

            foreach (PvPGameSpeedButton speedButton in speedButtons)
            {
                speedButton.Initialise(soundPlayer, shouldBeEnabledFilter, TimeBC.Instance);
            }

            IToggleButtonGroup speedButtonGroup = new PvPToggleButtonGroup(speedButtons.ToList<IToggleButton>(), normalSpeed);

            Highlightable speedButtonPanel = GetComponent<Highlightable>();
            Assert.IsNotNull(speedButtonPanel);
            speedButtonPanel.Initialise();

            return
                new PvPSpeedComponents(
                    speedButtonPanel,
                    speedButtonGroup,
                    slowMotion,
                    normalSpeed,
                    fastForward,
                    pause);
        }
    }
}