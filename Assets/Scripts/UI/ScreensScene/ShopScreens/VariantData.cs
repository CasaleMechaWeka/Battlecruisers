using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class VariantData : IVariantData
    {
        public int variantCost;
        public int VariantCost => variantCost;

        public string variantPrefabName;
        public string VariantPrefabName => variantPrefabName;

        public string variantNameStringKeyBase;
        public string VariantNameStringKeyBase => variantNameStringKeyBase;

        public string variantDescriptionStringKeyBase;
        public string VariantDescriptionStringKeyBase =>  variantDescriptionStringKeyBase;

        public bool isOwned;
        public bool IsOwned => isOwned;

        public int index;
        public int Index => index;

        public VariantData(string prefabName = "AntiAirTurret", string variantNameBase = "Variant000", string variantDescriptionBase = "VariantDescription000", int cost = 0, bool owned = false, int id = 0)
        {
            variantPrefabName = prefabName;
            variantCost = cost; 
            variantNameStringKeyBase = variantNameBase;
            variantDescriptionStringKeyBase = variantDescriptionBase;
            index = id;
            isOwned = owned;
        }
    }
}
