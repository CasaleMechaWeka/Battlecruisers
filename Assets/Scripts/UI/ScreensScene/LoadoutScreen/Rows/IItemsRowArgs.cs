using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IItemsRowArgs<TItem> where TItem : IComparableItem
    {
        IDataProvider DataProvider { get; }
        IPrefabFactory PrefabFactory {  get; }
        IItemDetailsManager<TItem> DetailsManager { get; }
    }
}
