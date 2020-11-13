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
        public IGameSpeedButton PlayButton { get; }
        public IGameSpeedButton FastForwardButton { get; }

        public SpeedComponents(
            IHighlightable speedButtonPanel,
            IToggleButtonGroup speedButtonGroup,
            IGameSpeedButton slowMotionButton,
            IGameSpeedButton playButton,
            IGameSpeedButton fastForwardButton)
        {
            Helper.AssertIsNotNull(speedButtonPanel, speedButtonGroup, slowMotionButton, playButton, fastForwardButton);

            SpeedButtonPanel = speedButtonPanel;
            SpeedButtonGroup = speedButtonGroup;
            SlowMotionButton = slowMotionButton;
            PlayButton = playButton;
            FastForwardButton = fastForwardButton;
        }
    }
}