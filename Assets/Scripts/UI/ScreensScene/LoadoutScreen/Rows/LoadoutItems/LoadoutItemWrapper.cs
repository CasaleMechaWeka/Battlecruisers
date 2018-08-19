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
        private TKey _itemKey;
        private IGameModel _gameModel;
        private LoadoutItem<TItem> _unlockedItem;
        private LockedItem _lockedItem;

        public void Initialise(TItem item, TKey itemKey , IItemDetailsManager<TItem> itemDetailsManager, IGameModel gameModel)
        {
            Helper.AssertIsNotNull(item, itemKey, itemDetailsManager, gameModel);

            _itemKey = itemKey;
            _gameModel = gameModel;

            _unlockedItem = GetComponent<LoadoutItem<TItem>>();
            Assert.IsNotNull(_unlockedItem);
            _unlockedItem.Initialise(item, itemDetailsManager);

            _lockedItem = GetComponent<LockedItem>();
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