using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public enum ItemType
    {
        // Cruisers
        Hull,
        // Captains
        Captains,
        // Buildings
        Factory, Defense, Offensive, Tactical, Ultra,
        // Units
        Ship, Aircraft
    }

    public enum ItemFamily
    {
        Hulls, Captains, Buildings, Units
    }

    public interface IItemsPanel : IPanel
    {
        ItemType ItemType { get; }
        bool HasUnlockedItem { get; }

        IItemButton GetFirstItemButton();
    }
}