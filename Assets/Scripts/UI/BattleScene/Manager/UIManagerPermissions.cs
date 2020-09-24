namespace BattleCruisers.UI.BattleScene.Manager
{
    public class UIManagerPermissions : IUIManagerPermissions, IUIManagerSettablePermissions
    {
        public bool CanShowItemDetails { get; set; }
        public bool CanDismissItemDetails { get; set; }
    }
}
