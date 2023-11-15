using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public interface IPvPSpeedComponents
    {
        IPvPHighlightable SpeedButtonPanel { get; }
        IPvPToggleButtonGroup SpeedButtonGroup { get; }
        IPvPGameSpeedButton PauseButton { get; }
        IPvPGameSpeedButton SlowMotionButton { get; }
        IPvPGameSpeedButton NormalSpeedButton { get; }
        IPvPGameSpeedButton FastForwardButton { get; }
    }
}