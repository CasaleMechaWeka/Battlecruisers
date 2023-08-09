using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public class HeckleData : Prefab, IHeckleData
    {
        public int heckleCost;
        public int HeckleCost => heckleCost;

        public string stringKeyBase;
        public string StringKeyBase => stringKeyBase;
    }
}

