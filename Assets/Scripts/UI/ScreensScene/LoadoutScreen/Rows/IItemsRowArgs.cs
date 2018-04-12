using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Data;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IItemsRowArgs<TItem> where TItem : IComparableItem
    {
        IGameModel GameModel { get; }
        ILockedInformation LockedInfo { get; }
        IPrefabFactory PrefabFactory {  get; }
        IUIFactory UIFactory { get; }
        IItemDetailsManager<TItem> DetailsManager { get; }
    }
}
