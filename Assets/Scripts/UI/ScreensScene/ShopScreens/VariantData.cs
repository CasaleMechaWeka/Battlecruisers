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

        public string perkNameStringKeyBase;
        public string PerkNameStringKeyBase => perkNameStringKeyBase;

        public string perkDescriptionStringKeyBase;
        public string PerkDescriptionStringKeyBase =>  perkDescriptionStringKeyBase;

        public bool isOwned;
        public bool IsOwned => isOwned;

        public int index;
        public int Index => index;

        public VariantData(string prefabName = "Variant000", string perkNameBase = "Perk000", string perkDescriptionBase = "PerkDescription000", int cost = 0, bool owned = false, int id = 0)
        {
            variantPrefabName = prefabName;
            variantCost = cost; 
            perkNameStringKeyBase = perkNameBase;
            perkDescriptionStringKeyBase = perkDescriptionBase;
            index = id;
            isOwned = owned;
        }
    }
}
