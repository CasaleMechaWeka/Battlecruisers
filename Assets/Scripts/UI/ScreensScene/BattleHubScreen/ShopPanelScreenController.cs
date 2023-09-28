using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using UnityEngine.UI;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Services.Core;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ShopPanelScreenController : ScreenController
    {
        public CanvasGroupButton backButton, /*buyCaptainButton, buyHeckleButton,*/ blackMarketButton;
        public CanvasGroupButton captainsButton, hecklesButton;
        public Transform captainItemContainer, heckleItemContainer;
        public GameObject captainItemPrefab, heckleItemPrefab;
        public CaptainsContainer captainsContainer;
        public HecklesContainer hecklesContainer;
        public GameObject hecklesMessage;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        public Transform captainCamContainer;
        private ILocTable commonStrings;
        public Image captainsButtonImage, hecklesButtonImage;
        public Text blackMarketText;
        private bool InternetConnection;

        public async Task Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(backButton, /*buyCaptainButton, buyHeckleButton,*/ blackMarketButton, captainsContainer);
            Helper.AssertIsNotNull(captainsButton, hecklesButton);
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;
            //Initialise each button with its function
            backButton.Initialise(_soundPlayer, GoHome, this);
            /*            buyCaptainButton.Initialise(_soundPlayer, PurchaseCaptainExo, this);
                        buyHeckleButton.Initialise(_soundPlayer, PurchaseHeckle, this);*/
            captainsButton.Initialise(_soundPlayer, CaptainsButton_OnClick);
            hecklesButton.Initialise(_soundPlayer, HeckesButton_OnClick);
            captainsContainer.Initialize(_soundPlayer, _dataProvider, _prefabFactory);
            hecklesContainer.Initialize(_soundPlayer, _dataProvider, _prefabFactory);
            commonStrings = LandingSceneGod.Instance.commonStrings;
            HighlightCaptainsNavButton();

            InternetConnection = await LandingSceneGod.CheckForInternetConnection();
            if (UnityServices.State != ServicesInitializationState.Uninitialized && InternetConnection)
            {
                // Only make cash shop available if there's an internet connection
                // Without one, we can't process transactions.
                blackMarketButton.Initialise(_soundPlayer, GotoBlackMarket, this);
                blackMarketText.text = LandingSceneGod.Instance.screenSceneStrings.GetString("BlackMarketOpen");
            }
            else
            {
                blackMarketButton.gameObject.SetActive(false);
            }
        }

        void OnEnable()
        {
            HighlightCaptainsNavButton();
        }

        //All the button fucntions for shop screen
        public void GoHome()
        {
            _screensSceneGod.GotoHubScreen();
        }

        public void GotoBlackMarket()
        {
            _screensSceneGod.GotoBlackMarketScreen();
        }

        public void CaptainsButton_OnClick()
        {
            InitiaiseCaptains();

            HighlightCaptainsNavButton();
        }

        public void HeckesButton_OnClick()
        {
            InitialiseHeckles();

            HighlightHecklesNavButton();
        }

        private void HighlightCaptainsNavButton()
        {
            // Set this button to active color
            captainsButtonImage.color = new Color32(255, 255, 255, 255);

            // Set the other button to inactive color
            hecklesButtonImage.color = new Color32(194, 59, 33, 255);
        }

        private void HighlightHecklesNavButton()
        {
            // Set this button to active color
            hecklesButtonImage.color = new Color32(255, 255, 255, 255);

            // Set the other button to inactive color
            captainsButtonImage.color = new Color32(194, 59, 33, 255);
        }

        private void RemoveAllCaptainsFromRenderCamera()
        {
            foreach (GameObject obj in captainsContainer.visualOfCaptains)
            {
                if (obj != null)
                    DestroyImmediate(obj);
            }
            captainsContainer.visualOfCaptains.Clear();
        }
        public async void InitialiseHeckles()
        {

            captainsContainer.gameObject.SetActive(false);
            hecklesMessage.gameObject.SetActive(true);
            hecklesContainer.gameObject.SetActive(true);
            // remove all old children to refresh
            HeckleItemController[] items = heckleItemContainer.gameObject.GetComponentsInChildren<HeckleItemController>();
            foreach (HeckleItemController item in items)
            {
                DestroyImmediate(item.gameObject);
            }

            RemoveAllCaptainsFromRenderCamera();


            hecklesContainer.btnBuy.SetActive(false);
            hecklesContainer.ownFeedback.SetActive(false);

            CaptainExo captainExo = Instantiate(_prefabFactory.GetCaptainExo(_dataProvider.GameModel.PlayerLoadout.CurrentCaptain), captainCamContainer);
            captainExo.gameObject.transform.localScale = Vector3.one * 0.5f;
            captainsContainer.visualOfCaptains.Add(captainExo.gameObject);

            await Task.Delay(100);

            DateTime utcNow = DateTime.UtcNow;
            List<int> heckleBaseList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            for (int i = 0; i < heckleBaseList.Count; i++)
            {
                heckleBaseList[i] = (19 * heckleBaseList[i] + 10 * utcNow.Day + utcNow.Month) % 279;
            }

            byte ii = 0;
            foreach (int index in heckleBaseList)
            {
                GameObject heckleItem = Instantiate(heckleItemPrefab, heckleItemContainer) as GameObject;
                heckleItem.GetComponent<HeckleItemController>().StaticInitialise(_soundPlayer, _dataProvider.GameModel.Heckles[index], hecklesContainer, ii);
                if (ii == 0)
                {
                    heckleItem.GetComponent<HeckleItemController>()._clickedFeedback.SetActive(true);
                    hecklesContainer.currentItem = heckleItem.GetComponent<HeckleItemController>();

                    heckleItem.GetComponent<HeckleItemController>().OnClicked();
                    hecklesContainer.hecklePrice.text = _dataProvider.GameModel.Heckles[index].heckleCost.ToString();
                    hecklesContainer.currentHeckleData = _dataProvider.GameModel.Heckles[index];
                    hecklesContainer.t_heckleMessage.text = LandingSceneGod.Instance.hecklesStrings.GetString(_dataProvider.GameModel.Heckles[index].StringKeyBase);

                    if (_dataProvider.GameModel.Heckles[index].IsOwned)
                    {
                        hecklesContainer.hecklePrice.text = "0";
                        hecklesContainer.btnBuy.SetActive(false);
                        hecklesContainer.ownFeedback.SetActive(true);
                    }
                    else
                    {
                        hecklesContainer.btnBuy.SetActive(true);
                        hecklesContainer.ownFeedback.SetActive(false);
                    }
                }
                ii++;
            }

        }

        public override void OnDismissing()
        {
            base.OnDismissing();
            RemoveAllCaptainsFromRenderCamera();
        }
        public async void InitiaiseCaptains()
        {

            captainsContainer.gameObject.SetActive(true);
            hecklesContainer.gameObject.SetActive(false);
            hecklesMessage.gameObject.SetActive(false);
            // remove all old children to refersh
            CaptainItemController[] items = captainItemContainer.gameObject.GetComponentsInChildren<CaptainItemController>();
            foreach (CaptainItemController item in items)
            {
                DestroyImmediate(item.gameObject);
            }
            captainsContainer.btnBuy.SetActive(false);
            captainsContainer.ownFeedback.SetActive(false);
            await Task.Delay(100);


            RemoveAllCaptainsFromRenderCamera();

            DateTime utcNow = DateTime.UtcNow;
            List<int> exoBaseList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            for (int i = 0; i < exoBaseList.Count; i++)
            {
                exoBaseList[i] = 1 + ((2 * exoBaseList[i] + utcNow.Day + utcNow.Month) % 39);
            }
            exoBaseList.Insert(0, 0);

            byte ii = 0;
            foreach (int index in exoBaseList)
            {
                GameObject captainItem = Instantiate(captainItemPrefab, captainItemContainer) as GameObject;
                CaptainExo captainExo = Instantiate(_prefabFactory.GetCaptainExo(StaticPrefabKeys.CaptainExos.AllKeys[index]), captainCamContainer);
                captainExo.gameObject.transform.localScale = Vector3.one * 0.5f;
                captainExo.gameObject.SetActive(false);
                captainsContainer.visualOfCaptains.Add(captainExo.gameObject);
                captainItem.GetComponent<CaptainItemController>().StaticInitialise(_soundPlayer, captainExo.CaptainExoImage, _dataProvider.GameModel.Captains[index], captainsContainer, ii);

                if (ii == 0)  // the first item should be clicked :)
                {
                    captainItem.GetComponent<CaptainItemController>()._clickedFeedback.SetActive(true);
                    captainsContainer.currentItem = captainItem.GetComponent<CaptainItemController>();
                    //    captainItem.GetComponent<CaptainItemController>().OnClicked(); // to display Captain's price
                    if (index == 0)
                    {
                        captainsContainer.captainPrice.text = "0"; // CaptainExo000 is default item. :)
                        captainsContainer.captainName.text = commonStrings.GetString(_dataProvider.GameModel.Captains[0].nameStringKeyBase);
                        captainsContainer.captainDescription.text = commonStrings.GetString(_dataProvider.GameModel.Captains[0].descriptionKeyBase);
                    }
                    captainExo.gameObject.SetActive(true);
                    if (_dataProvider.GameModel.Captains[index].IsOwned)
                    {
                        captainsContainer.btnBuy.SetActive(false);
                        captainsContainer.ownFeedback.SetActive(true);
                    }
                    else
                    {
                        captainsContainer.btnBuy.SetActive(true);
                        captainsContainer.ownFeedback.SetActive(false);
                    }
                }
                ii++;
            }
        }
    }
}