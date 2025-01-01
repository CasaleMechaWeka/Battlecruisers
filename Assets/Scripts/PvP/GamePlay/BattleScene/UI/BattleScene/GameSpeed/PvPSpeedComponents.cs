using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public class PvPSpeedComponents : IPvPSpeedComponents
    {
        public IHighlightable SpeedButtonPanel { get; }
        public IPvPToggleButtonGroup SpeedButtonGroup { get; }
        public IPvPGameSpeedButton SlowMotionButton { get; }
        public IPvPGameSpeedButton NormalSpeedButton { get; }
        public IPvPGameSpeedButton FastForwardButton { get; }
        public IPvPGameSpeedButton PauseButton { get; }

        public PvPSpeedComponents(
            IHighlightable speedButtonPanel,
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