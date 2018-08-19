using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public abstract class LoadoutItemWrapper<TItem, TKey> : MonoBehaviour, IStatefulUIElement
        where TItem : class, IComparableItem
        where TKey : class, IPrefabKey
    {
        private LoadoutItem<TItem> _unlockedItem;
        private LockedItem _lockedItem;
        protected TKey _itemKey;
        protected IGameModel _gameModel;

        public void Initialise(TItem item, TKey itemKey , IItemDetailsManager<TItem> itemDetailsManager, IGameModel gameModel)
        {
            Helper.AssertIsNotNull(item, itemKey, itemDetailsManager, gameModel);

            _itemKey = itemKey;
            _gameModel = gameModel;

            _unlockedItem = GetComponentInChildren<LoadoutItem<TItem>>();
            Assert.IsNotNull(_unlockedItem);
            _unlockedItem.Initialise(item, itemDetailsManager);

            _lockedItem = GetComponentInChildren<LockedItem>();
            Assert.IsNotNull(_lockedItem);
        }

        public void RefreshLockedStatus()
        {
            bool isItemUnlocked = IsItemUnlocked();

            _unlockedItem.IsVisible = isItemUnlocked;
            _lockedItem.IsVisible = !isItemUnlocked;
        }

        protected abstract bool IsItemUnlocked();

        public void GoToState(UIState state)
        {
            _unlockedItem.GoToState(state);
        }
    }
}