using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public abstract class LoadoutItemWrapper<TItem, TKey> : ItemWrapper<TItem, TKey>
        where TItem : class, IComparableItem
        where TKey : class, IPrefabKey
    {
        private LoadoutItem<TItem> _unlockedItem;
        protected override IItem<TItem> UnlockedItem { get { return _unlockedItem; } }

        public void Initialise(TItem item, TKey itemKey , IItemDetailsManager<TItem> itemDetailsManager, IGameModel gameModel)
        {
            base.Initialise(gameModel, itemKey);

            Helper.AssertIsNotNull(item, itemDetailsManager);

            _unlockedItem = GetComponentInChildren<LoadoutItem<TItem>>();
            Assert.IsNotNull(_unlockedItem);
            _unlockedItem.Initialise(item, itemDetailsManager);
        }
    }
}