using System;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class HeckleData : IHeckleData
    {
        public int heckleCost;
        public int HeckleCost => heckleCost;

        public string stringKeyBase;
        public string StringKeyBase => stringKeyBase;
        public int index;
        public int Index => index;

        public HeckleData(string keyBase = "Heckle000", int cost = 0, int id = 0)
        {
            stringKeyBase = keyBase;
            heckleCost = cost;
            index = id;
        }
    }
}

