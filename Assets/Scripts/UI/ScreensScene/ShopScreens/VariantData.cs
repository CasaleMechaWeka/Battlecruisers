using System;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class VariantData : IVariantData
    {
        public int variantCoins;
        public int VariantCoins => variantCoins;

        public int variantCredits;
        public int VariantCredits => variantCredits;

        public string variantPrefabName;
        public string VariantPrefabName => variantPrefabName;

        public string variantNameStringKeyBase;
        public string VariantNameStringKeyBase => variantNameStringKeyBase;

        public string variantDescriptionStringKeyBase;
        public string VariantDescriptionStringKeyBase => variantDescriptionStringKeyBase;

        public bool isOwned;
        public bool IsOwned => isOwned;

        public int index;
        public int Index => index;

        public VariantData(string prefabName = "Variant000", string variantNameBase = "Variant000", string variantDescriptionBase = "VariantDescription000", int coins = 0, int credits = 0, bool owned = false, int id = 0)
        {
            variantPrefabName = prefabName;
            variantCoins = coins;
            variantCredits = credits;
            variantNameStringKeyBase = variantNameBase;
            variantDescriptionStringKeyBase = variantDescriptionBase;
            index = id;
            isOwned = owned;
        }
    }
}
