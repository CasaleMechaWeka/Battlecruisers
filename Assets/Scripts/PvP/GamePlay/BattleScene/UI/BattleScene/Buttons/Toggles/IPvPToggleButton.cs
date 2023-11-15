namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles
{
    public interface IPvPToggleButton : IPvPButton
    {
        bool IsSelected { set; get; }
    }
}