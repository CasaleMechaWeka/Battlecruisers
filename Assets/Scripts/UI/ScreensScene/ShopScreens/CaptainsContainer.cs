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
        public Text captainDescription;
        public Text captainPrice;
        public EventHandler<CaptainDataEventArgs> captainDataChanged;
        private ILocTable commonStrings;
        public CaptainItemController currentItem;
        public List<GameObject> visualOfCaptains = new List<GameObject>();
        public GameObject btnBuy, ownFeedback;
        public void Initialize()
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            captainDataChanged += CaptainDataChanged;
        }

        private void CaptainDataChanged(object sender, CaptainDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            visualOfCaptains[currentItem._index].SetActive(false);
            currentItem = (CaptainItemController)sender;
            if(e.captainData.IsOwned)
            {
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }
               
        //    captainImage.sprite = e.captainImage;
            captainName.text = commonStrings.GetString(e.captainData.NameStringKeyBase);
            captainDescription.text = commonStrings.GetString(e.captainData.DescriptionKeyBase);
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

