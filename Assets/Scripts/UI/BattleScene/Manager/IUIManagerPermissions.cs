namespace BattleCruisers.UI.BattleScene.Manager
{
    public interface IUIManagerPermissions
    {
        bool CanShowItemDetails { get; }
        bool CanDismissItemDetails { get; }
    }
}
