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

        public bool isOwned;
        public bool IsOwned => isOwned;

        public int index;
        public int Index => index;

        public string descriptionKeyBase;
        public string DescriptionKeyBase => descriptionKeyBase;



        public BodykitData(string nameBase = "Bodykit000", string descriptionBase = "BodykitDescription000", int cost = 0, bool owned = false, int id = 0)
        {
            nameStringKeyBase = nameBase;
            bodykitCost = cost;
            isOwned = owned;
            index = id;
            descriptionKeyBase = descriptionBase;
        }
    }
}


