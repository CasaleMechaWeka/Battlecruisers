using System;

namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class IAPData
    {
        public int IAPType { get; }

        public string IAPNameKeyBase { get; }

        public string IAPDescriptionKeyBase { get; }

        public string IAPIconName { get; }

        public float IAPCost { get; set; }

        public int IAPCoins { get; }

        public IAPData(int iapType = 0, float iapCost = 0.99f, int iapCoins = 100)
        {
            IAPType = iapType;
            IAPCost = iapCost;
            IAPCoins = iapCoins;
            IAPNameKeyBase = "Coins" + IAPCoins + "Name";
            IAPDescriptionKeyBase = "Coins" + IAPCoins + "Description";
            IAPIconName = "Coins" + IAPCoins + "Pack";
        }
    }
}
