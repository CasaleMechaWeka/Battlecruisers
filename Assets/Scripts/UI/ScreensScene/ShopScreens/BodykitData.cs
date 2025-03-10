using System;

namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]

    public class BodykitData : IBodykitData
    {
        public int bodykitCost;
        public int BodykitCost => bodykitCost;

        public string nameStringKeyBase;
        public string NameStringKeyBase => nameStringKeyBase;

        public int index;
        public int Index => index;

        public string descriptionKeyBase;
        public string DescriptionKeyBase => descriptionKeyBase;



        public BodykitData(string nameBase = "Bodykit000", string descriptionBase = "BodykitDescription000", int cost = 0, int id = 0)
        {
            nameStringKeyBase = nameBase;
            bodykitCost = cost;
            index = id;
            descriptionKeyBase = descriptionBase;
        }
    }
}


