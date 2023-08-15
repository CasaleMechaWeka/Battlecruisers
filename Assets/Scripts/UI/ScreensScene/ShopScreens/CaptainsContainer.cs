using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
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

        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider)
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            captainDataChanged += CaptainDataChanged;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
        }

        private void Purchase()
        {
            
        }

        private async void CaptainDataChanged(object sender, CaptainDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            visualOfCaptains[currentItem._index].SetActive(false);
            currentItem = (CaptainItemController)sender;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
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
            captainPrice.text = (await _dataProvider.GetCaptainCost(e.captainData.Index)).ToString();
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

