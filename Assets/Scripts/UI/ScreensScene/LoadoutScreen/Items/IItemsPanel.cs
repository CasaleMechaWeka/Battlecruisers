using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public enum ItemType
    {
        Hull,
        // Buildings
        Factory, Defense, Offensive, Tactical, Ultra,
        // Units
        Ship, Aircraft
    }

    public enum ItemFamily
    {
        Hulls, Buildings, Units
    }

    public enum ItemTypeCapacity
    {
        Factory = 5, Defense = 5, Offensive = 5, Tactical = 5, Ultra = 5, Ship = 5, Aircraft = 5
    }

    public interface IItemsPanel : IPanel
    {
        ItemType ItemType { get; }
        bool HasUnlockedItem { get; }
    }
}