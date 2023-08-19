using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainExo : Prefab, ICaptainExo
    {
        public Sprite captainExoImage;
        public Sprite CaptainExoImage => captainExoImage;

        public string captainName;
        public string NameKeyBase => captainName;
    }
}