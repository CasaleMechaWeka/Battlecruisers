using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public interface IPvPSpeedComponents
    {
        IHighlightable SpeedButtonPanel { get; }
        IPvPToggleButtonGroup SpeedButtonGroup { get; }
        IPvPGameSpeedButton PauseButton { get; }
        IPvPGameSpeedButton SlowMotionButton { get; }
        IPvPGameSpeedButton NormalSpeedButton { get; }
        IPvPGameSpeedButton FastForwardButton { get; }
    }
}