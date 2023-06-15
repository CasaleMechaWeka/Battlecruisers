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

        public string stringKeyBase;
        public string StringKeyBase => stringKeyBase;

        //public void Initialise(ILocTable captainStrings)
        //{
        //    Assert.IsNotNull(captainStrings);
        //    captainName = 
        //}
    }
}