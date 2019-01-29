using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class ItemsPanel : Panel, IItemsPanel
    {
        public ItemType itemType;
        public ItemType ItemType { get { return itemType; } }

        public void Initialise(IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingFamiltyTracker)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker);

            ItemButton[] buttons = GetComponentsInChildren<ItemButton>(includeInactive: true);

            foreach (ItemButton button in buttons)
            {
                button.Initialise(itemDetailsManager, comparingFamiltyTracker);
            }
        }
    }
}