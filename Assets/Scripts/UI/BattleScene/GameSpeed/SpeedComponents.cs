using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class SpeedComponents : ISpeedComponents
    {
        public IHighlightable SpeedButtonPanel { get; }
        public IToggleButtonGroup SpeedButtonGroup { get; }
        public IGameSpeedButton SlowMotionButton { get; }
        public IGameSpeedButton NormalSpeedButton { get; }
        public IGameSpeedButton FastForwardButton { get; }
        public IGameSpeedButton PauseButton { get; }

        public SpeedComponents(
            IHighlightable speedButtonPanel,
            IToggleButtonGroup speedButtonGroup,
            IGameSpeedButton slowMotionButton,
            IGameSpeedButton normalSpeedButton,
            IGameSpeedButton fastForwardButton)
        {
            Helper.AssertIsNotNull(speedButtonPanel, speedButtonGroup, slowMotionButton, normalSpeedButton, fastForwardButton);

            SpeedButtonPanel = speedButtonPanel;
            SpeedButtonGroup = speedButtonGroup;
            SlowMotionButton = slowMotionButton;
            NormalSpeedButton = normalSpeedButton;
            FastForwardButton = fastForwardButton;
        }
    }
}