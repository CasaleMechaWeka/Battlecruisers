using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    /// <summary>
    /// Wraps:
    /// 1. Unlocked item (eg, a hull, building or unit)
    /// 2. Locked item (placeholder with a locked symbol)
    /// 
    /// If the user has unlocked the item we show the unlocked item, otherwise
    /// we show the locked item placeholder.
    /// </summary>
    public abstract class ItemWrapper<TItem, TPrefabKey> : MonoBehaviour, IStatefulUIElement
        where TItem : IComparableItem
        where TPrefabKey : class, IPrefabKey
    {
        private IGameObject _lockedItem;
        protected IGameModel _gameModel;
        protected TPrefabKey _itemKey;

        protected abstract IItem<TItem> UnlockedItem { get; }

        public void Initialise(IGameModel gameModel, TPrefabKey itemKey)
        {
            Helper.AssertIsNotNull(gameModel, itemKey);

            _gameModel = gameModel;
            _itemKey = itemKey;

            _lockedItem = GetComponentInChildren<LockedItem>();
            Assert.IsNotNull(_lockedItem);
        }

        // FELIX  Rename method?  OnPresented :)
        public void RefreshLockedStatus()
        {
            bool isItemUnlocked = IsItemUnlocked();

            UnlockedItem.IsVisible = isItemUnlocked;
            _lockedItem.IsVisible = !isItemUnlocked;

            UnlockedItem.ShowSelectedFeedback = false;
        }

        protected abstract bool IsItemUnlocked();

        public void GoToState(UIState state)
        {
            UnlockedItem.GoToState(state);
        }
    }
}