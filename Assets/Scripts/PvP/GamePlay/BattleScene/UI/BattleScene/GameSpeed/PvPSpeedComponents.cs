using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.GameSpeed;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public class PvPSpeedComponents : IPvPSpeedComponents
    {
        public IHighlightable SpeedButtonPanel { get; }
        public IPvPToggleButtonGroup SpeedButtonGroup { get; }
        public IGameSpeedButton SlowMotionButton { get; }
        public IGameSpeedButton NormalSpeedButton { get; }
        public IGameSpeedButton FastForwardButton { get; }
        public IGameSpeedButton PauseButton { get; }

        public PvPSpeedComponents(
            IHighlightable speedButtonPanel,
            IPvPToggleButtonGroup speedButtonGroup,
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