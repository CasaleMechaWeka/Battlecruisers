using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class CaptainData : ICaptainData
    {
        public int captainCost;
        public int CaptainCost => captainCost;

        public string nameStringKeyBase;
        public string NameStringKeyBase => nameStringKeyBase;

        public bool isOwned;
        public bool IsOwned => isOwned;

        public int index;
        public int Index => index;

        public string descriptionKeyBase;
        public string DescriptionKeyBase => descriptionKeyBase; 

        public CaptainData(string nameBase = "CaptainExo000", string descriptionBase = "CaptainDescription000", int cost = 0, bool owned = false, int id = 0)
        {
            nameStringKeyBase = nameBase;
            captainCost = cost;
            isOwned = owned;
            index = id;
            descriptionKeyBase = descriptionBase;
        }
    }
}


