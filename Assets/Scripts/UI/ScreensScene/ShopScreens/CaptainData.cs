using System;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class CaptainData : ICaptainData
    {
        public int CaptainCost { get; set; }

        public string NameStringKeyBase { get; }

        public int Index { get; }

        public string DescriptionKeyBase { get; }

        public CaptainData(int cost = 0, int id = 0)
        {
            CaptainCost = cost;
            Index = id;
            //this formats it to CaptainExo000, CaptainExo0001, ...
            NameStringKeyBase = "CaptainExo" + Index.ToString("D3");
            //this formats it to CaptainDescription000, CaptainDescription0001, ...
            DescriptionKeyBase = "CaptainDescription" + Index.ToString("D3");
        }
    }
}


