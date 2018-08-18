using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class ItemsRowArgs<TItem> : IItemsRowArgs<TItem> where TItem : IComparableItem
    {
        public IDataProvider DataProvider { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
        public IUIFactory UIFactory { get; private set; }
        public IItemDetailsManager<TItem> DetailsManager { get; private set; }

        public ItemsRowArgs(
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory,
            IUIFactory uiFactory,
            IItemDetailsManager<TItem> detailsManager)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, uiFactory, detailsManager);

            DataProvider = dataProvider;
            PrefabFactory = prefabFactory;
            UIFactory = uiFactory;
            DetailsManager = detailsManager;
        }
    }
}
