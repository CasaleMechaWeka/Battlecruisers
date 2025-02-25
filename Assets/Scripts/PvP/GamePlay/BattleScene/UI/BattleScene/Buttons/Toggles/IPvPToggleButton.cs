using BattleCruisers.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles
{
    public interface IPvPToggleButton : IButton
    {
        bool IsSelected { set; get; }
    }
}