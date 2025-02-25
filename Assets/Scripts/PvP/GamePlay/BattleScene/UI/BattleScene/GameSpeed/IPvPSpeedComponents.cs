using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.BattleScene.GameSpeed;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public interface IPvPSpeedComponents
    {
        IHighlightable SpeedButtonPanel { get; }
        IToggleButtonGroup SpeedButtonGroup { get; }
        IGameSpeedButton PauseButton { get; }
        IGameSpeedButton SlowMotionButton { get; }
        IGameSpeedButton NormalSpeedButton { get; }
        IGameSpeedButton FastForwardButton { get; }
    }
}