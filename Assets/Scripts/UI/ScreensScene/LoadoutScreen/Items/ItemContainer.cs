using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
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
        public IItemButton Initialise(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IGameModel gameModel,
            IBroadcastingProperty<HullKey> selectedHull,
            ISoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamilyTracker, gameModel, selectedHull, soundPlayer);

            LockedItem lockedItem = GetComponentInChildren<LockedItem>(includeInactive: true);
            Assert.IsNotNull(lockedItem);

            ItemButton itemButton = InitialiseItemButton(itemDetailsManager, comparingFamilyTracker, selectedHull, soundPlayer);

            bool isItemUnlocked = IsUnlocked(gameModel);
            lockedItem.IsVisible = !isItemUnlocked;
            itemButton.IsVisible = isItemUnlocked;

            return itemButton;
        }

        protected virtual ItemButton InitialiseItemButton(
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamilyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            ISoundPlayer soundPlayer)
        {
            ItemButton itemButton = GetComponentInChildren<ItemButton>(includeInactive: true);
            Assert.IsNotNull(itemButton);
            itemButton.Initialise(soundPlayer, itemDetailsManager, comparingFamilyTracker);
            return itemButton;
        }

        protected abstract bool IsUnlocked(IGameModel gameModel);
    }
}