using System;

namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class VariantData
    {
        public int VariantCoins { get; set; }

        public int VariantCredits { get; set; }

        public string VariantPrefabName { get; }

        public string VariantNameStringKeyBase { get; }

        public string VariantDescriptionStringKeyBase { get; }

        public int Index { get; }

        public VariantData(string variantNameBase, int coins = 0, int credits = 0, int id = 0)
        {

            VariantCoins = coins;
            VariantCredits = credits;
            Index = id;
            VariantPrefabName = "Variant" + Index.ToString("D3");
            VariantNameStringKeyBase = variantNameBase;
            VariantDescriptionStringKeyBase = VariantNameStringKeyBase + "Description";
        }
    }
}
