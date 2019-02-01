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

    public interface IItemsPanel : IPanel
    {
        ItemType ItemType { get; }
        bool HasUnlockedItem { get; }
    }
}