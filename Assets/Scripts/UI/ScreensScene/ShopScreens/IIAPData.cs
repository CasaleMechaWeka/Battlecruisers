using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IIAPData
    {
        int IAPType { get; } // 0: coin, 1,,,
        string IAPNameKeyBase { get; }
        string IAPDescriptionKeyBase { get; }
        string IAPIconName { get; }
        float IAPCost { get; }
        int IAPCoins { get; }

    }
}
