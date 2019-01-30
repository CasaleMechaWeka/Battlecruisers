using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
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
    public abstract class ItemWrapper<TItem, TPrefabKey> : PresentableController, IStatefulUIElement
        where TItem : IComparableItem
        where TPrefabKey : class, IPrefabKey
    {
        private IGameObject _lockedItem;
        protected IGameModel _gameModel;
        protected TPrefabKey _itemKey;

        protected abstract IItem<TItem> UnlockedItem { get; }

        public void Initialise(IGameModel gameModel, TPrefabKey itemKey)
        {
            base.Initialise();

            Helper.AssertIsNotNull(gameModel, itemKey);

            _gameModel = gameModel;
            _itemKey = itemKey;

            _lockedItem = GetComponentInChildren<LockedItem>();
            Assert.IsNotNull(_lockedItem);
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

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