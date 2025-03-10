using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.BattleScene.GameSpeed;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public class PvPSpeedComponents : ISpeedComponents
    {
        public IHighlightable SpeedButtonPanel { get; }
        public IToggleButtonGroup SpeedButtonGroup { get; }
        public IGameSpeedButton SlowMotionButton { get; }
        public IGameSpeedButton NormalSpeedButton { get; }
        public IGameSpeedButton FastForwardButton { get; }
        public IGameSpeedButton PauseButton { get; }

        public PvPSpeedComponents(
            IHighlightable speedButtonPanel,
            IToggleButtonGroup speedButtonGroup,
            IGameSpeedButton slowMotionButton,
            IGameSpeedButton normalSpeedButton,
            IGameSpeedButton fastForwardButton,
            IGameSpeedButton pauseButton)
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