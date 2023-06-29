using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainData : Prefab, ICaptainData
    {
        public Sprite captainImage;
        public Sprite CaptainImage => captainImage;

        public string captainName = "Charlie";
        public string CaptainName => captainName;

        public int captainCost;
        public int CaptainCost => captainCost;

        public string stringKeyBase;
        public string StringKeyBase => stringKeyBase;

        // CaptainIndex is the order of which the captains appear in the story mode.
        public int captainIndex;
        public int CaptainIndex => captainIndex;

        public bool IsOwned { get; set; }//To check if player owns them
                                        
        //public void Initialise(ILocTable captainStrings)
        //{
        //    Assert.IsNotNull(captainStrings);
        //    captainName = 
        //}
    }
}