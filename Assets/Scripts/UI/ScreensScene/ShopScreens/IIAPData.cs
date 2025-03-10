namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IIAPData
    {
        int IAPType { get; } // 0: coin, 1,,,
        string IAPNameKeyBase { get; }
        string IAPDescriptionKeyBase { get; }
        string IAPIconName { get; }
        float IAPCost { get; set; }
        int IAPCoins { get; }

    }
}
