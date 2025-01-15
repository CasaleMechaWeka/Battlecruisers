namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IVariantData
    {
        int VariantCoins { get; }
        int VariantCredits { get; }
        string VariantPrefabName { get; }
        string VariantNameStringKeyBase { get; }
        string VariantDescriptionStringKeyBase { get; }
        int Index { get; }
    }
}
