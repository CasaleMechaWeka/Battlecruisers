namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public interface ISpeedPanelHighlighter
    {
        void UnhighlightButtons();
        void HighlightSlowMotionButton();
        void HighlightFastForwardButton();
    }
}