using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class HecklesContainer : MonoBehaviour
    {
        public EventHandler<HeckleDataEventArgs> heckleDataChanged;
        public EventHandler<HeckleDataEventArgs> onHeckleItemClick;

        public Text t_heckleMessage;
        public GameObject obj_heckleMessage;
        public GameObject btnBuy, ownFeedback;
        public HeckleItemController currentItem;
        public HeckleData currentHeckleData;
        public Text hecklePrice;
        public GameObject priceLabel;
        private ISingleSoundPlayer _soundPlayer;
        private PrefabFactory _prefabFactory;

        public void Initialize(ISingleSoundPlayer soundPlayer, PrefabFactory prefabFactory)
        {
            heckleDataChanged += HeckleDataChanged;
            _soundPlayer = soundPlayer;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            priceLabel = hecklePrice.transform.parent.gameObject;
        }

        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if (DataProvider.GameModel.Coins >= currentHeckleData.HeckleCost)
            {
                if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                {
                    // Online purchasing
                    try
                    {
                        bool result = await DataProvider.PurchaseHeckleV2(currentHeckleData.Index);
                        if (result)
                        {
                            //    await DataProvider.SyncCurrencyFromCloud();
                            PlayerInfoPanelController.Instance.UpdateInfo(_prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            DataProvider.GameModel.AddHeckle(currentHeckleData.Index);
                            DataProvider.SaveGame();
                            await DataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            if (LocTableCache.HecklesTable.GetString(currentHeckleData.StringKeyBase).Length <= 10)
                            {
                                // For heckles with 10 or less characters!
                                ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("HecklePurchased") + " \"" + LocTableCache.HecklesTable.GetString(currentHeckleData.StringKeyBase));
                            }
                            else
                            {
                                ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("HecklePurchased") + " \"" + LocTableCache.HecklesTable.GetString(currentHeckleData.StringKeyBase).Substring(0, 10) + "...\"");
                            }
                            ScreensSceneGod.Instance.loadoutScreen.AddHeckle(currentHeckleData);
                            priceLabel.SetActive(false);

                            string logName = currentHeckleData.StringKeyBase;
#if LOG_ANALYTICS
                Debug.Log("Analytics: " + logName);
#endif
                            Dictionary<string, object> transactionDetails = new Dictionary<string, object>() { { "heckleIndex", currentHeckleData.Index } };
                        }
                        else
                        {
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("TryAgain"));
                        }
                    }
                    catch
                    {
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("TryAgain"));
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
                        DataProvider.GameModel.AddHeckle(currentHeckleData.Index);
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        if (LocTableCache.HecklesTable.GetString(currentHeckleData.StringKeyBase).Length <= 10)
                        {
                            // For heckles with 10 or less characters!
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("HecklePurchased") + " \"" + LocTableCache.HecklesTable.GetString(currentHeckleData.StringKeyBase));
                        }
                        else
                        {
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("HecklePurchased") + " \"" + LocTableCache.HecklesTable.GetString(currentHeckleData.StringKeyBase).Substring(0, 10) + "...\"");
                        }
                        priceLabel.SetActive(false);

                        // Subtract from local economy:
                        DataProvider.GameModel.Coins -= currentHeckleData.HeckleCost;
                        PlayerInfoPanelController.Instance.UpdateInfo(_prefabFactory);

                        // Keep track of transaction for later:
                        DataProvider.GameModel.CoinsChange -= currentHeckleData.HeckleCost;
                        HeckleData heckle = StaticData.Heckles[currentHeckleData.Index];
                        if (DataProvider.GameModel.OutstandingHeckleTransactions == null)
                        {
                            DataProvider.GameModel.OutstandingHeckleTransactions = new List<HeckleData>();
                        }
                        DataProvider.GameModel.OutstandingHeckleTransactions.Add(heckle);
                        DataProvider.SaveGame();
                    }
                    catch
                    {
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("TryAgain"));
                    }
                    ScreensSceneGod.Instance.processingPanel.SetActive(false);
                }
            }
            else
            {
                ScreensSceneGod.Instance.processingPanel.SetActive(false);

                // Check for Windows platform
#if UNITY_STANDALONE_WIN
                // Execute this line if it's a Windows build
                ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("InsufficientCoins"), null, null);
#else
                // Execute the original line for non-Windows builds
                ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("InsufficientCoins"), GotoBlackMarket, LocTableCache.ScreensSceneTable.GetString("GetCoins"));
#endif
            }
            Debug.Log(DataProvider.GameModel.Coins);

        }

        private void HeckleDataChanged(object sender, HeckleDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem = (HeckleItemController)sender;
            currentHeckleData = e.heckleData;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
            if (DataProvider.GameModel.PurchasedHeckles.Contains(e.heckleData.Index))
            {
                priceLabel.SetActive(false);
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                priceLabel.SetActive(true);
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }

            t_heckleMessage.text = LocTableCache.HecklesTable.GetString(e.heckleData.StringKeyBase);
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
        public HeckleData heckleData { get; set; }
    }
}
