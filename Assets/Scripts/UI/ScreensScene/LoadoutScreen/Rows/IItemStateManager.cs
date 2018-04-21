using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IItemStateManager
    {
        void AddItem(IStatefulUIElement itemToAdd, ItemType type);

        void HandleDetailsManagerDismissed();
        void HandleDetailsManagerReadyToCompare(ItemType comparingType);
        void HandleDetailsManagerComparing();
    }
}
