namespace BattleCruisers.UI.BattleScene.Manager
{
    public interface IUIManagerSettablePermissions
    {
        bool CanShowItemDetails { set; }
        bool CanDismissItemDetails { set; }
    }
}
