using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Authentication;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using Unity.Services.Core;

namespace BattleCruisers.UI.ScreensScene
{
    public class HecklesContainer : MonoBehaviour
    {
        private ILocTable hecklesStrings;
        public EventHandler<HeckleDataEventArgs> heckleDataChanged;

        public Text t_heckleMessage;
        public GameObject obj_heckleMessage;
        public GameObject btnBuy, ownFeedback;
        public HeckleItemController currentItem;
        public IHeckleData currentHeckleData;
        public Text hecklePrice;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private ILocTable screensSceneTable;

        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            hecklesStrings = LandingSceneGod.Instance.hecklesStrings;
            heckleDataChanged += HeckleDataChanged;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            screensSceneTable = LandingSceneGod.Instance.screenSceneStrings;
        }

        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if (_dataProvider.GameModel.Coins >= currentHeckleData.HeckleCost)
            {
                if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                {
                    // Online purchasing
                    try
                    {
                        bool result = await _dataProvider.PurchaseHeckleV2(currentHeckleData.Index);
                        if (result)
                        {

                        //    await _dataProvider.SyncCurrencyFromCloud();
                            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            _dataProvider.GameModel.Heckles[currentHeckleData.Index].isOwned = true;
                            _dataProvider.GameModel.AddHeckle(currentHeckleData.Index);
                            _dataProvider.SaveGame();
                            await _dataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            if (hecklesStrings.GetString(currentHeckleData.StringKeyBase).Length <= 10)
                            {
                                // For heckles with 10 or less characters!
                                ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("HecklePurchased") + " \"" + hecklesStrings.GetString(currentHeckleData.StringKeyBase));
                            }
                            else
                            {
                                ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("HecklePurchased") + " \"" + hecklesStrings.GetString(currentHeckleData.StringKeyBase).Substring(0, 10) + "...\"");
                            }
                            ScreensSceneGod.Instance.loadoutScreen.AddHeckle(currentHeckleData);

                            string logName = currentHeckleData.StringKeyBase;
#if LOG_ANALYTICS
                Debug.Log("Analytics: " + logName);
#endif
                            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
                            Dictionary<string, object> transactionDetails = new Dictionary<string, object>() { { "heckleIndex", currentHeckleData.Index } };
                            try
                            {
                                AnalyticsService.Instance.CustomData("Shop_Heckle_Bought", transactionDetails);
                                AnalyticsService.Instance.Flush();
                            }
                            catch (ConsentCheckException ex)
                            {
                                Debug.Log(ex.Message);
                            }


                        }
                        else
                        {
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("TryAgain"));
                        }
                    }
                    catch
                    {
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("TryAgain"));
                    }
                    ScreensSceneGod.Instance.processingPanel.SetActive(false);
                }
                else
                {
                    // Offline purchasing
                    try
                    {
                        currentItem._clickedFeedback.SetActive(true);
                        currentItem._ownedItemMark.SetActive(true);
                        btnBuy.SetActive(false);
                        ownFeedback.SetActive(true);
                        ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                        _dataProvider.GameModel.Heckles[currentHeckleData.Index].isOwned = true;
                        _dataProvider.GameModel.AddHeckle(currentHeckleData.Index);
                        _dataProvider.SaveGame();
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        if (hecklesStrings.GetString(currentHeckleData.StringKeyBase).Length <= 10)
                        {
                            // For heckles with 10 or less characters!
                            ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("HecklePurchased") + " \"" + hecklesStrings.GetString(currentHeckleData.StringKeyBase));
                        }
                        else
                        {
                            ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("HecklePurchased") + " \"" + hecklesStrings.GetString(currentHeckleData.StringKeyBase).Substring(0, 10) + "...\"");
                        }

                        // Subtract from local economy:
                        _dataProvider.GameModel.Coins -= currentHeckleData.HeckleCost;
                        PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);

                        // Keep track of transaction for later:
                        _dataProvider.GameModel.CoinsChange -= currentHeckleData.HeckleCost;
                        HeckleData heckle = _dataProvider.GameModel.Heckles[currentHeckleData.Index];
                        if (_dataProvider.GameModel.OutstandingHeckleTransactions == null)
                        {
                            _dataProvider.GameModel.OutstandingHeckleTransactions = new List<HeckleData>();
                        }
                        _dataProvider.GameModel.OutstandingHeckleTransactions.Add(heckle);
                    }
                    catch
                    {
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("TryAgain"));
                    }
                    ScreensSceneGod.Instance.processingPanel.SetActive(false);
                }
            }
            else
            {
                ScreensSceneGod.Instance.processingPanel.SetActive(false);
                ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("InsufficientCoins"), GotoBlackMarket, screensSceneTable.GetString("GetCoins"));
                return;
            }
        }

        private void HeckleDataChanged(object sender, HeckleDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem = (HeckleItemController)sender;
            currentHeckleData = e.heckleData;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
            if (e.heckleData.IsOwned)
            {
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }

            t_heckleMessage.text = hecklesStrings.GetString(e.heckleData.StringKeyBase);
            hecklePrice.text = e.heckleData.HeckleCost.ToString();
            obj_heckleMessage.GetComponent<RectTransform>().localScale = Vector3.zero;
            obj_heckleMessage.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f);
        }

        private void OnDestroy()
        {
            heckleDataChanged -= HeckleDataChanged;
        }

        public void GotoBlackMarket()
        {
            GetComponentInParent<ShopPanelScreenController>().GotoBlackMarket();
        }

    }

    public class HeckleDataEventArgs : EventArgs
    {
        public IHeckleData heckleData { get; set; }
    }
}
