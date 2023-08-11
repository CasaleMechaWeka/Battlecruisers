using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class CaptainsContainer : MonoBehaviour
    {
        public Image captainImage;
        public Text captainName;
        public Text captainPrice;
        public EventHandler<CaptainDataEventArgs> captainDataChanged;
        ILocTable commonStrings;
        public void Initialize()
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            captainDataChanged += CaptainDataChanged;
        }

        private void CaptainDataChanged(object sender, CaptainDataEventArgs e)
        {
            captainImage.sprite = e.captainImage;
            captainName.text = commonStrings.GetString(e.captainData.NameStringKeyBase);
            captainPrice.text = e.captainData.CaptainCost.ToString("#,##0");
        }

        private void OnDestroy()
        {
            captainDataChanged -= CaptainDataChanged;
        }
    }

    public class CaptainDataEventArgs : EventArgs
    {
        public ICaptainData captainData { get; set; }
        public Sprite captainImage { get; set; }
    }
}

