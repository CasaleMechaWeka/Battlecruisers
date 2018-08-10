using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using UnityEngine.Assertions;
using BattleCruisers.Data;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public abstract class ItemsRow<TItem> : IItemsRow<TItem> where TItem : class, IComparableItem
    {
        protected readonly IGameModel _gameModel;
		protected readonly ILockedInformation _lockedInfo;
		protected readonly IPrefabFactory _prefabFactory;
        protected readonly IUIFactory _uiFactory;
        protected readonly IItemDetailsManager<TItem> _detailsManager;

        protected ItemsRow(IItemsRowArgs<TItem> args)
		{
            Assert.IsNotNull(args);

			_gameModel = args.GameModel;
            _lockedInfo = args.LockedInfo;
            _prefabFactory = args.PrefabFactory;
            _uiFactory = args.UIFactory;
            _detailsManager = args.DetailsManager;
		}

        public abstract void SetupUI();
        public abstract bool SelectUnlockedItem(UnlockedItem<TItem> item);
        public abstract void GoToState(UIState state);
    }
}
