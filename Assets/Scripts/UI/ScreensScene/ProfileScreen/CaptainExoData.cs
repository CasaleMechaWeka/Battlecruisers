using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainExoData : Prefab, ICaptainExoData
    {
        public Sprite captainExoImage;
        public Sprite CaptainExoImage => captainExoImage;

        public string captainExoName = "Charlie";
        public string CaptainExoName => captainExoName;

        public int captainExoCost;
        public int CaptainExoCost => captainExoCost;

        private string stringKeyBase;
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