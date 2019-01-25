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

    public interface IItemsPanel : IPanel
    {
        ItemType ItemType { get; }
    }
}