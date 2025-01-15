namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface ICaptainData
    {
        int CaptainCost { get; set; }
        string NameStringKeyBase { get; }
        string DescriptionKeyBase { get; }
        int Index { get; }
    }
}