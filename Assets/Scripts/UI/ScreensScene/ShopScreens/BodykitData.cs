using System;

namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]

    public class BodykitData
    {
        public int BodykitCost { get; set; }
        public string NameStringKeyBase { get; }
        public int Index { get; }

        public string DescriptionKeyBase { get; }

        public BodykitData(string nameBase = "Bodykit000", string descriptionBase = "BodykitDescription000", int cost = 0, int id = 0)
        {
            NameStringKeyBase = nameBase;
            BodykitCost = cost;
            Index = id;
            DescriptionKeyBase = descriptionBase;
        }
    }
}