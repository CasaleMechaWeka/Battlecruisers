using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IHeckleData
    {
        int HeckleCost { get; }
        string StringKeyBase { get; }        
    }
}

