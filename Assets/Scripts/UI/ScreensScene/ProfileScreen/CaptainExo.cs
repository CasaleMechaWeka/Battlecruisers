using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainExo : Prefab
    {
        public Sprite captainExoImage;
        public Sprite CaptainExoImage => captainExoImage;

        private string _captainName;
        public string captainName
        {
            get => _captainName;
            set
            {
                Debug.Log($"Setting captainName to: {value}");
                _captainName = value;
            }
        }

        public string NameKeyBase => captainName;
    }
}
