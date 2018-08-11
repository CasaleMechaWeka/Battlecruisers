using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IHullItemsRow : IItemsRow<ICruiser>, IPresentable
    {
        HullKey UserChosenHull { get; }
    }
}
