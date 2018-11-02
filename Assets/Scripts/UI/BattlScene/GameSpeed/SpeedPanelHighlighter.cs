using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class SpeedPanelHighlighter : ISpeedPanelHighlighter
    {
        private readonly IGameObject _slowMotionButtonHighlight, _fastForwardButtonHighlight;

        public SpeedPanelHighlighter(IGameObject slowMotionButtonHighlight, IGameObject fastForwardButtonHighlight)
        {
            Helper.AssertIsNotNull(slowMotionButtonHighlight, fastForwardButtonHighlight);

            _slowMotionButtonHighlight = slowMotionButtonHighlight;
            _fastForwardButtonHighlight = fastForwardButtonHighlight;
        }

        public void UnhighlightButtons()
        {
            _slowMotionButtonHighlight.IsVisible = false;
            _fastForwardButtonHighlight.IsVisible = false;
        }

        public void HighlightSlowMotionButton()
        {
            UnhighlightButtons();
            _slowMotionButtonHighlight.IsVisible = true;
        }

        public void HighlightFastForwardButton()
        {
            UnhighlightButtons();
            _fastForwardButtonHighlight.IsVisible = true;
        }
    }
}