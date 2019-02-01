using BattleCruisers.Data;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public abstract class ItemsRow<TItem> : Presentable, IItemsRow<TItem> 
        where TItem : class, IComparableItem
    {
        protected readonly IDataProvider _dataProvider;
		protected readonly IPrefabFactory _prefabFactory;
        protected readonly IItemDetailsManager<TItem> _detailsManager;

        protected ItemsRow(IItemsRowArgs<TItem> args)
		{
            Assert.IsNotNull(args);

            _dataProvider = args.DataProvider;
            _prefabFactory = args.PrefabFactory;
            _detailsManager = args.DetailsManager;
		}

        public abstract bool SelectUnlockedItem(UnlockedItem<TItem> item);
        public abstract void GoToState(UIState state);
    }
}
