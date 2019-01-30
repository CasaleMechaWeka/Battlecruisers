using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    /// <summary>
    /// Contains:
    /// + Item button
    /// + Locked item placeholder
    /// 
    /// Shows the item button if the item has been unlocked, otherwise shows
    /// the locked placholder.
    /// </summary>
    public abstract class ItemContainer : MonoBehaviour
    {
        public bool Initialise(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IGameModel gameModel)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker, gameModel);

            LockedItem lockedItem = GetComponentInChildren<LockedItem>(includeInactive: true);
            Assert.IsNotNull(lockedItem);

            ItemButton itemButton = GetComponentInChildren<ItemButton>(includeInactive: true);
            Assert.IsNotNull(itemButton);
            itemButton.Initialise(itemDetailsManager, comparingFamiltyTracker);

            bool isItemUnlocked = IsUnlocked(gameModel);
            lockedItem.IsVisible = !isItemUnlocked;
            itemButton.IsVisible = isItemUnlocked;

            return isItemUnlocked;
        }

        protected abstract bool IsUnlocked(IGameModel gameModel);
    }
}