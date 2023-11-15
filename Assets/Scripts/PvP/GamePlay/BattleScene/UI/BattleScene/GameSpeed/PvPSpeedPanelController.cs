using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public class PvPSpeedPanelController : MonoBehaviour
    {
        public PvPGameSpeedButton slowMotion, normalSpeed, fastForward, pause;

        public PvPSpeedComponents Initialise(IPvPSingleSoundPlayer soundPlayer, IPvPBroadcastingFilter shouldBeEnabledFilter)
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
                speedButton.Initialise(soundPlayer, shouldBeEnabledFilter, PvPTimeBC.Instance);
            }

            IPvPToggleButtonGroup speedButtonGroup = new PvPToggleButtonGroup(speedButtons.ToList<IPvPToggleButton>(), normalSpeed);

            PvPHighlightable speedButtonPanel = GetComponent<PvPHighlightable>();
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