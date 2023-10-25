using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class BodykitsContainer : MonoBehaviour
    {
        public Image bodykitImage;
        public Text bodykitName;
        public Text bodykitDescription;
        public Text bodykitPrice;
        public EventHandler<BodykitDataEventArgs> bodykitDataChanged;
        private ILocTable commonStrings;
        public BodykitItemController currentItem;
        private IBodykitData currentBodykitData;
        public GameObject btnBuy, ownFeedback;
        private string firstNameString;
        private string firstDescriptionString;

        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        public GameObject content;
        private ILocTable screensSceneTable;

        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            bodykitDataChanged += BodykitDataChanged;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            screensSceneTable = LandingSceneGod.Instance.screenSceneStrings;
        }
        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if (_dataProvider.GameModel.Coins >= currentBodykitData.BodykitCost)
            {
                if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                {
                    // online purchase

                }
                else
                {
                    // offline purchase

                }
            }
            else
            {
                ScreensSceneGod.Instance.processingPanel.SetActive(false);
                ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("InsufficientCoins"));
                return;
            }
        }
        private void BodykitDataChanged(object sender, BodykitDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem = (BodykitItemController)sender;
            currentBodykitData = e.bodykitData;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
            if (e.bodykitData.IsOwned)
            {
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }

            bodykitPrice.text = e.bodykitData.BodykitCost.ToString();
            bodykitImage.sprite = e.bodykitImage;
            bodykitName.text = commonStrings.GetString(e.bodykitData.NameStringKeyBase);
            bodykitDescription.text = commonStrings.GetString(e.bodykitData.DescriptionKeyBase);
        }
        private void OnDestroy()
        {
            bodykitDataChanged -= BodykitDataChanged;
        }
    }

    public class BodykitDataEventArgs : EventArgs
    {
        public IBodykitData bodykitData { get; set; }
        public Sprite bodykitImage { get; set; }
    }
}

