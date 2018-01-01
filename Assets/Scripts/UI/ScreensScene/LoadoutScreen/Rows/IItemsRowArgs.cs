using BattleCruisers.Data.Models;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IItemsRowArgs<TItem> where TItem : IComparableItem
    {
        IGameModel GameModel { get; }
        IPrefabFactory PrefabFactory {  get; }
        IUIFactory UIFactory { get; }
        IItemDetailsManager<TItem> DetailsManager { get; }
    }
}
