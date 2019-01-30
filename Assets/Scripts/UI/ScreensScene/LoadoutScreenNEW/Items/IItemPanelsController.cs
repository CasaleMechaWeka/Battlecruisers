using BattleCruisers.UI.Filters;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public interface IItemPanelsController : IBroadcastingFilter<ItemType>
    {
        void ShowItemsPanel(ItemType itemType);
        IItemsPanel GetPanel(ItemType itemType);
    }
}