using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IItemsRowArgs<TItem> where TItem : IComparableItem
    {
        IDataProvider DataProvider { get; }
        IPrefabFactory PrefabFactory {  get; }
        IUIFactory UIFactory { get; }
        IItemDetailsManager<TItem> DetailsManager { get; }
    }
}
