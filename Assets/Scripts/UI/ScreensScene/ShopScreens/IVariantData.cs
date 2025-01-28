namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IVariantData
    {
        int VariantCoins { get; set; }
        int VariantCredits { get; set; }
        string VariantPrefabName { get; }
        string VariantNameStringKeyBase { get; }
        string VariantDescriptionStringKeyBase { get; }
        int Index { get; }
    }
}
