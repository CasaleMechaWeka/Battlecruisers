using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainExoButton : MonoBehaviour
    {

        public Button button;
        public Image captainImage;
        public Toggle activeCaptainToggle;

        private CaptainExoKey captainKey;

        public void Initialize(CaptainExoKey key, Sprite captainSprite, bool isActiveCaptain)
        {
            captainKey = key;
            captainImage.sprite = captainSprite;
            activeCaptainToggle.isOn = isActiveCaptain;

            button.onClick.AddListener(SetCurrentCaptain);
        }

        private void SetCurrentCaptain()
        {
            // Code here to set the current captain in GameModel to this button's captain
        }

    }
}

