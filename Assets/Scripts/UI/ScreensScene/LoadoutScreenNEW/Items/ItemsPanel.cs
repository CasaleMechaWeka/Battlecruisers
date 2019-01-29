using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class ItemsPanel : Panel, IItemsPanel
    {
        public ItemType itemType;
        public ItemType ItemType { get { return itemType; } }

        public void Initialise(IItemDetailsManager itemDetailsManager, ISettableBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            Helper.AssertIsNotNull(itemDetailsManager, itemFamilyToCompare);

            ItemButton[] buttons = GetComponentsInChildren<ItemButton>(includeInactive: true);

            foreach (ItemButton button in buttons)
            {
                button.Initialise(itemDetailsManager, itemFamilyToCompare);
            }
        }
    }
}