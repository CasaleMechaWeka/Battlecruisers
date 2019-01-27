using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class ItemsPanel : Panel, IItemsPanel
    {
        public ItemType itemType;
        public ItemType ItemType { get { return itemType; } }

        public void Initialise(IItemDetailsDisplayer itemDetailsDisplayer, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            Helper.AssertIsNotNull(itemDetailsDisplayer, itemFamilyToCompare);

            ItemButton[] buttons = GetComponentsInChildren<ItemButton>(includeInactive: true);

            foreach (ItemButton button in buttons)
            {
                button.Initialise(itemDetailsDisplayer, itemFamilyToCompare);
            }
        }
    }
}