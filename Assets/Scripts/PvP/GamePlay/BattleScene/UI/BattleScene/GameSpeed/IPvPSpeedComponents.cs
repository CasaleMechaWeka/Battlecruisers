using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.GameSpeed;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public interface IPvPSpeedComponents
    {
        IHighlightable SpeedButtonPanel { get; }
        IPvPToggleButtonGroup SpeedButtonGroup { get; }
        IGameSpeedButton PauseButton { get; }
        IGameSpeedButton SlowMotionButton { get; }
        IGameSpeedButton NormalSpeedButton { get; }
        IGameSpeedButton FastForwardButton { get; }
    }
}