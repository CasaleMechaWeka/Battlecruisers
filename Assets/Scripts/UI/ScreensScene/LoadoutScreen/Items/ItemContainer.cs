using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    /// <summary>
    /// Contains:
    /// + Item button
    /// + Locked item placeholder
    /// + New item mark
    /// 
    /// Shows the item button if the item has been unlocked, otherwise shows
    /// the locked placholder.
    /// </summary>
    public abstract class ItemContainer : MonoBehaviour
    {
        private IGameModel _gameModel;
        private NewItemMark _newItemMark;

        public IItemButton Initialise(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IGameModel gameModel,
            IBroadcastingProperty<HullKey> selectedHull,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamilyTracker, gameModel, selectedHull, soundPlayer, prefabFactory);

            _gameModel = gameModel;

            LockedItem lockedItem = GetComponentInChildren<LockedItem>(includeInactive: true);
            Assert.IsNotNull(lockedItem);

            _newItemMark = GetComponentInChildren<NewItemMark>(includeInactive: true);
            Assert.IsNotNull(_newItemMark);

            ItemButton itemButton = InitialiseItemButton(itemDetailsManager, comparingFamilyTracker, selectedHull, soundPlayer, prefabFactory);
            itemButton.Clicked += ItemButton_Clicked;

            bool isItemUnlocked = IsUnlocked(gameModel);
            lockedItem.IsVisible = !isItemUnlocked;
            itemButton.IsVisible = isItemUnlocked;

            UpdateNewItemMarkVisibility();

            return itemButton;
        }

        protected abstract ItemButton InitialiseItemButton(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory);

        private void ItemButton_Clicked(object sender, EventArgs e)
        {
            Logging.LogMethod(Tags.LOADOUT_SCREEN);
            MakeOld(_gameModel);
            UpdateNewItemMarkVisibility();
        }

        protected abstract bool IsUnlocked(IGameModel gameModel);
        protected abstract bool IsNew(IGameModel gameModel);
        protected abstract void MakeOld(IGameModel gameModel);

        private void UpdateNewItemMarkVisibility()
        {
            Logging.Log(Tags.LOADOUT_SCREEN, $"_newItemMark.IsVisible: {_newItemMark.IsVisible}");
            _newItemMark.IsVisible = IsNew(_gameModel);
            Logging.Log(Tags.LOADOUT_SCREEN, $"_newItemMark.IsVisible: {_newItemMark.IsVisible}");
        }
    }
}