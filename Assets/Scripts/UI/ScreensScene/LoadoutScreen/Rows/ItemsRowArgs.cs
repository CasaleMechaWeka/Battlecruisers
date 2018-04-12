using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Data;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class ItemsRowArgs<TItem> : IItemsRowArgs<TItem> where TItem : IComparableItem
    {
        public IGameModel GameModel { get; private set; }
        public ILockedInformation LockedInfo { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
        public IUIFactory UIFactory { get; private set; }
        public IItemDetailsManager<TItem> DetailsManager { get; private set; }

        public ItemsRowArgs(
            IGameModel gameModel,
            ILockedInformation lockedInfo,
            IPrefabFactory prefabFactory,
            IUIFactory uiFactory,
            IItemDetailsManager<TItem> detailsManager)
        {
            Helper.AssertIsNotNull(gameModel, lockedInfo, prefabFactory, uiFactory, detailsManager);

            GameModel = gameModel;
            LockedInfo = lockedInfo;
            PrefabFactory = prefabFactory;
            UIFactory = uiFactory;
            DetailsManager = detailsManager;
        }
    }
}
