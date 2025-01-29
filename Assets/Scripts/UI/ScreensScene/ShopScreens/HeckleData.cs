using System;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class HeckleData : IHeckleData
    {
        public int HeckleCost { get; set; }
        public string StringKeyBase { get; }
        public int Index { get; }

        public HeckleData(int cost = 0, int id = 0)
        {
            HeckleCost = cost;
            Index = id;
            //this formats it to Heckle000, Heckle0001, ...
            StringKeyBase = "Heckle" + Index.ToString("D3");
        }
    }
}

