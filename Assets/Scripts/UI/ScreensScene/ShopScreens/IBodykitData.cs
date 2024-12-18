namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IBodykitData
    {
        int BodykitCost { get; }
        string NameStringKeyBase { get; }
        string DescriptionKeyBase { get; }
        bool IsOwned { get; }
        int Index { get; }
    }
}
