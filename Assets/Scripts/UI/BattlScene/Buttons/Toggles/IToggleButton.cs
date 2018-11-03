namespace BattleCruisers.UI.BattleScene.Buttons.Toggles
{
    public interface IToggleButton : IButton
    {
        bool IsSelected { set; }
    }
}