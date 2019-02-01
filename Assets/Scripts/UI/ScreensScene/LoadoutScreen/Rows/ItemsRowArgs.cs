using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class ItemsRowArgs<TItem> : IItemsRowArgs<TItem> where TItem : IComparableItem
    {
        public IDataProvider DataProvider { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
        public IItemDetailsManager<TItem> DetailsManager { get; private set; }

        public ItemsRowArgs(
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory,
            IItemDetailsManager<TItem> detailsManager)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, detailsManager);

            DataProvider = dataProvider;
            PrefabFactory = prefabFactory;
            DetailsManager = detailsManager;
        }
    }
}
