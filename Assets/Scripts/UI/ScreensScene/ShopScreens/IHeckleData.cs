namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IHeckleData
    {
        int HeckleCost { get; }
        string StringKeyBase { get; }
        bool IsOwned { get; }
        int Index { get; }
    }
}

