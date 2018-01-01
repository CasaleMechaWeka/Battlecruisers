using BattleCruisers.Data.Models;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class ItemsRowArgs<TItem> : IItemsRowArgs<TItem> where TItem : IComparableItem
    {
        public IGameModel GameModel { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
        public IUIFactory UIFactory { get; private set; }
        public IItemDetailsManager<TItem> DetailsManager { get; private set; }

        public ItemsRowArgs(
            IGameModel gameModel,
            IPrefabFactory prefabFactory,
            IUIFactory uiFactory,
            IItemDetailsManager<TItem> detailsManager)
        {
            Helper.AssertIsNotNull(gameModel, prefabFactory, uiFactory, detailsManager);

            GameModel = gameModel;
            PrefabFactory = prefabFactory;
            UIFactory = uiFactory;
            DetailsManager = detailsManager;
        }
    }
}
