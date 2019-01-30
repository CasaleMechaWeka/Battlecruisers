using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class ItemsPanel : Panel, IItemsPanel
    {
        public ItemType itemType;
        public ItemType ItemType { get { return itemType; } }

        public bool HasUnlockedItem { get; private set; }

        public void Initialise(
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IGameModel gameModel)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker, gameModel);

            ItemContainer[] itemContainers = GetComponentsInChildren<ItemContainer>(includeInactive: true);

            HasUnlockedItem = false;

            foreach (ItemContainer itemContainer in itemContainers)
            {
                bool isItemUnlocked = itemContainer.Initialise(itemDetailsManager, comparingFamiltyTracker, gameModel);
                HasUnlockedItem = HasUnlockedItem || isItemUnlocked;
            }
        }
    }
}