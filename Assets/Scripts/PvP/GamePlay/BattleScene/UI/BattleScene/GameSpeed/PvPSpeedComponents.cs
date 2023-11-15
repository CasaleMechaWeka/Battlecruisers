using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public class PvPSpeedComponents : IPvPSpeedComponents
    {
        public IPvPHighlightable SpeedButtonPanel { get; }
        public IPvPToggleButtonGroup SpeedButtonGroup { get; }
        public IPvPGameSpeedButton SlowMotionButton { get; }
        public IPvPGameSpeedButton NormalSpeedButton { get; }
        public IPvPGameSpeedButton FastForwardButton { get; }
        public IPvPGameSpeedButton PauseButton { get; }

        public PvPSpeedComponents(
            IPvPHighlightable speedButtonPanel,
            IPvPToggleButtonGroup speedButtonGroup,
            IPvPGameSpeedButton slowMotionButton,
            IPvPGameSpeedButton normalSpeedButton,
            IPvPGameSpeedButton fastForwardButton,
            IPvPGameSpeedButton pauseButton)
        {
            PvPHelper.AssertIsNotNull(speedButtonPanel, speedButtonGroup, slowMotionButton, normalSpeedButton, fastForwardButton);

            SpeedButtonPanel = speedButtonPanel;
            SpeedButtonGroup = speedButtonGroup;
            SlowMotionButton = slowMotionButton;
            NormalSpeedButton = normalSpeedButton;
            FastForwardButton = fastForwardButton;
            PauseButton = pauseButton;
        }
    }
}