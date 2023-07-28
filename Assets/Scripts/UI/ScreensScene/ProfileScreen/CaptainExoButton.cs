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
        public Image activeCaptainImage;

        private CaptainExoKey captainKey;
        private Action<CaptainExoKey> setCurrentCaptainAction;

        public void Initialize(CaptainExoKey key, Sprite captainSprite, bool isActiveCaptain, Action<CaptainExoKey> setCurrentCaptainAction)
        {
            captainKey = key;
            captainImage.sprite = captainSprite;
            activeCaptainImage.gameObject.SetActive(isActiveCaptain);
            this.setCurrentCaptainAction = setCurrentCaptainAction;

            button.onClick.AddListener(SetCurrentCaptain);
        }

        private void SetCurrentCaptain()
        {
            setCurrentCaptainAction?.Invoke(captainKey);
        }

        public void UpdateActiveState(CaptainExoKey currentCaptain)
        {
            activeCaptainImage.gameObject.SetActive(captainKey == currentCaptain);
        }
    }
}
