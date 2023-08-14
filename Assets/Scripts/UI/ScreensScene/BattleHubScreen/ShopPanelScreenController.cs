using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

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
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        public Transform captainCamContainer;


        public void Initialise(
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
            blackMarketButton.Initialise(_soundPlayer, GotoBlackMarket, this);
            captainsContainer.Initialize(_soundPlayer);
            hecklesContainer.Initialize(_soundPlayer);
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
        }

        public void HeckesButton_OnClick()
        {
            InitialiseHeckles();
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


            // remove all old children to refresh
            HeckleItemController[] items = heckleItemContainer.gameObject.GetComponentsInChildren<HeckleItemController>();
            foreach (HeckleItemController item in items)
            {
                DestroyImmediate(item.gameObject);
            }

            RemoveAllCaptainsFromRenderCamera();

            captainsContainer.gameObject.SetActive(false);
            hecklesContainer.gameObject.SetActive(true);
            hecklesContainer.btnBuy.SetActive(false);
            hecklesContainer.ownFeedback.SetActive(false);

            CaptainExo captainExo = Instantiate(_prefabFactory.GetCaptainExo(_dataProvider.GameModel.PlayerLoadout.CurrentCaptain), captainCamContainer);
            captainExo.gameObject.transform.localScale = Vector3.one * 0.5f;
            captainsContainer.visualOfCaptains.Add(captainExo.gameObject);

            await Task.Delay(100);
            byte ii = 0;
            foreach (int index in _dataProvider.GameModel.HeckleList)
            {
                GameObject heckleItem = Instantiate(heckleItemPrefab, heckleItemContainer) as GameObject;
                heckleItem.GetComponent<HeckleItemController>().StaticInitialise(_soundPlayer, _dataProvider.GameModel.Heckles[index], hecklesContainer, ii);
                if (ii == 0)
                {
                    heckleItem.GetComponent<HeckleItemController>()._clickedFeedback.SetActive(true);
                    hecklesContainer.currentItem = heckleItem.GetComponent<HeckleItemController>();

                    if (_dataProvider.GameModel.Heckles[index].IsOwned)
                    {
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

            captainsContainer.gameObject.SetActive(true);
            hecklesContainer.gameObject.SetActive(false);

            byte ii = 0;
            foreach (int index in _dataProvider.GameModel.CaptainExoList)
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