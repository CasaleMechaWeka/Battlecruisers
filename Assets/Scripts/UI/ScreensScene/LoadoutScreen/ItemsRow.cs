using BattleCruisers.Data.Models;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public abstract class ItemsRow<TItem> : IItemsRow<TItem> where TItem : IComparableItem
	{
		protected readonly IGameModel _gameModel;
		protected readonly IPrefabFactory _prefabFactory;
        protected readonly IUIFactory _uiFactory;
        protected readonly IItemDetailsManager<TItem> _detailsManager;

        protected ItemsRow(
            IGameModel gameModel, 
            IPrefabFactory prefabFactory,
            IUIFactory uiFactory, 
            IItemDetailsManager<TItem> detailsManager)
		{
            Helper.AssertIsNotNull(gameModel, prefabFactory, uiFactory, detailsManager);

			_gameModel = gameModel;
			_prefabFactory = prefabFactory;
            _uiFactory = uiFactory;
            _detailsManager = detailsManager;
		}

        public abstract void SetupUI();
        public abstract bool SelectUnlockedItem(UnlockedItem<TItem> item);
    }
}
