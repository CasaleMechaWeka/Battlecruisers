using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class ItemsPanel : Panel, IItemsPanel
    {
        public ItemType itemType;
        public ItemType ItemType => itemType;

        public bool HasUnlockedItem { get; private set; }

        public IList<IItemButton> Initialise(
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IGameModel gameModel,
            IBroadcastingProperty<HullKey> selectedHull)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker, gameModel, selectedHull);

            ItemContainer[] itemContainers = GetComponentsInChildren<ItemContainer>(includeInactive: true);
            IList<IItemButton> buttons = new List<IItemButton>();

            HasUnlockedItem = false;

            foreach (ItemContainer itemContainer in itemContainers)
            {
                IItemButton button = itemContainer.Initialise(itemDetailsManager, comparingFamiltyTracker, gameModel, selectedHull);
                buttons.Add(button);
                HasUnlockedItem = HasUnlockedItem || button.IsUnlocked;
            }

            return buttons;
        }
    }
}