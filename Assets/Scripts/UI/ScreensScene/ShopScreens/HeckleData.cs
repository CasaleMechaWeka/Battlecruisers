using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class HeckleData : IHeckleData
    {
        public int heckleCost;
        public int HeckleCost => heckleCost;

        public string stringKeyBase;
        public string StringKeyBase => stringKeyBase;

        public bool isOwned;
        public bool IsOwned => isOwned;

        public int index;
        public int Index => index;

        public HeckleData(string keyBase = "Heckle000", int cost = 0, bool owned = false, int id = 0)
        {
            stringKeyBase = keyBase;
            heckleCost = cost;
            isOwned = owned;
            index = id;
        }
    }
}

