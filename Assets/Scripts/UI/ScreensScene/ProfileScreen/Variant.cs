using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class Variant : Prefab, IVariant
    {
        public Sprite _variantSprite;
        public Sprite variantSprite => _variantSprite;

        public VariantType _variantType;
        public VariantType variantType => _variantType;
    }
}
