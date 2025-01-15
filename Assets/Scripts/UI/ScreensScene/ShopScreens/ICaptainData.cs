namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface ICaptainData
    {
        int CaptainCost { get; }
        string NameStringKeyBase { get; }
        string DescriptionKeyBase { get; }
        int Index { get; }
    }
}