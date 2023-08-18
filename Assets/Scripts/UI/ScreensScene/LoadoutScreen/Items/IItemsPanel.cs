using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public enum ItemType
    {
        // Cruisers
        Hull,
        // Buildings
        Factory, Defense, Offensive, Tactical, Ultra,
        // Units
        Ship, Aircraft,
        // Heckle
        Heckle
    }

    public enum ItemFamily
    {
        Hulls, Buildings, Units, Heckles
    }

    public interface IItemsPanel : IPanel
    {
        ItemType ItemType { get; }
        bool HasUnlockedItem { get; }

        IItemButton GetFirstItemButton();
    }
}