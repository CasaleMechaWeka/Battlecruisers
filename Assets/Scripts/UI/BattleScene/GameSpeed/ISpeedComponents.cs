using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public interface ISpeedComponents
    {
        IHighlightable SpeedButtonPanel { get; }
        IToggleButtonGroup SpeedButtonGroup { get; }
        IGameSpeedButton SlowMotionButton { get; }
        IGameSpeedButton NormalSpeedButton { get; }
        IGameSpeedButton FastForwardButton { get; }
    }
}