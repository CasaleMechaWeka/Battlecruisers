namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
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