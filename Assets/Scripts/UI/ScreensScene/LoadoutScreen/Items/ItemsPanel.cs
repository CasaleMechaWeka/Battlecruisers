using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class ItemsPanel : Panel, IItemsPanel
    {
        public ItemType itemType;
        public ItemType ItemType { get { return itemType; } }

        public bool HasUnlockedItem { get; private set; }

        public void Initialise(
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IGameModel gameModel,
            IBroadcastingProperty<HullKey> selectedHull)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker, gameModel, selectedHull);

            ItemContainer[] itemContainers = GetComponentsInChildren<ItemContainer>(includeInactive: true);

            HasUnlockedItem = false;

            foreach (ItemContainer itemContainer in itemContainers)
            {
                bool isItemUnlocked = itemContainer.Initialise(itemDetailsManager, comparingFamiltyTracker, gameModel, selectedHull);
                HasUnlockedItem = HasUnlockedItem || isItemUnlocked;
            }
        }
    }
}