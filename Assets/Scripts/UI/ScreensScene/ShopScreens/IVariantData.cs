using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IVariantData
    {
        int VariantCost { get; }
        string VariantPrefabName { get; }
        string VariantNameStringKeyBase { get; }
        string VariantDescriptionStringKeyBase { get; }
        bool IsOwned { get; }
        int Index { get; }
    }
}
