using BattleCruisers.Data.Models;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public abstract class ItemsRow<TItem> : IItemsRow<TItem> where TItem : IComparableItem
	{
		protected readonly IGameModel _gameModel;
		protected readonly IPrefabFactory _prefabFactory;

		public ItemsRow(IGameModel gameModel, IPrefabFactory prefabFactory)
		{
			_gameModel = gameModel;
			_prefabFactory = prefabFactory;
		}

		public abstract bool SelectUnlockedItem(UnlockedItem<TItem> item);
	}
}
