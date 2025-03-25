using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
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
        public EventHandler<CaptainDataEventArgs> onCaptainItemClick;
        public CaptainItemController currentItem;
        private ICaptainData currentCaptainData;
        public List<GameObject> visualOfCaptains = new List<GameObject>();
        public GameObject btnBuy, ownFeedback;
        public GameObject priceLabel;
        public GameObject ShopBuyControls;

        private string firstNameString;
        private string firstDescrtiptionString;

        private ISingleSoundPlayer _soundPlayer;
        private PrefabFactory _prefabFactory;
        public GameObject content;
        public GameObject captainMessagePanel;
        public GameObject itemDetailsPanel;
        public Text t_captainMessage;

        public void Initialize(ISingleSoundPlayer soundPlayer, PrefabFactory prefabFactory)
        {
            captainDataChanged += CaptainDataChanged;
            onCaptainItemClick += OnCaptainItemClick;
            _soundPlayer = soundPlayer;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            firstNameString = captainName.text;
            firstDescrtiptionString = captainDescription.text;
            priceLabel = captainPrice.transform.parent.gameObject;
            ShopBuyControls.SetActive(false);
        }

        private void OnEnable()
        {
            if (firstNameString != null)
                captainName.text = firstNameString;
            if (firstNameString != firstDescrtiptionString)
                captainDescription.text = firstDescrtiptionString;
        }

        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if (DataProvider.GameModel.Coins >= currentCaptainData.CaptainCost)
            {
                if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                {
                    // Online purchasing
                    try
                    {
                        bool result = await DataProvider.PurchaseCaptainV2(currentCaptainData.Index);
                        if (result)
                        {
                            //    await DataProvider.SyncCurrencyFromCloud();
                            PlayerInfoPanelController.Instance.UpdateInfo(_prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            DataProvider.GameModel.AddExo(currentCaptainData.Index);
                            DataProvider.SaveGame();
                            await DataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("CaptainExoPurchased") + " " + LocTableCache.CommonTable.GetString(currentCaptainData.NameStringKeyBase));
                            priceLabel.SetActive(false);
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

                    string logName = currentCaptainData.NameStringKeyBase;
#if LOG_ANALYTICS
                Debug.Log("Analytics: " + logName);
#endif
                    Dictionary<string, object> transactionDetails = new Dictionary<string, object>() { { "exoIndex", currentCaptainData.Index } };
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
                        DataProvider.GameModel.AddExo(currentCaptainData.Index);
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("CaptainExoPurchased") + " " + LocTableCache.CommonTable.GetString(currentCaptainData.NameStringKeyBase));
                        priceLabel.SetActive(false);

                        // Subtract from local economy:
                        DataProvider.GameModel.Coins -= currentCaptainData.CaptainCost;
                        PlayerInfoPanelController.Instance.UpdateInfo(_prefabFactory);

                        // Keep track of transaction for later:
                        DataProvider.GameModel.CoinsChange -= currentCaptainData.CaptainCost;
                        CaptainData captain = StaticData.Captains[currentCaptainData.Index];
                        if (DataProvider.GameModel.OutstandingCaptainTransactions == null)
                        {
                            DataProvider.GameModel.OutstandingCaptainTransactions = new List<CaptainData>();
                        }
                        DataProvider.GameModel.OutstandingCaptainTransactions.Add(captain);
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
        }

        private void OnCaptainItemClick(object sender, CaptainDataEventArgs e)
        {
            // Hide the message panel and show the item details
            if (captainMessagePanel.activeSelf)
            {
                captainMessagePanel.SetActive(false);
                itemDetailsPanel.SetActive(true);
            }
        }

        private void CaptainDataChanged(object sender, CaptainDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            visualOfCaptains[currentItem._index].SetActive(false);
            currentItem = (CaptainItemController)sender;
            currentCaptainData = e.captainData;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
            ShopBuyControls.SetActive(true);
            if (DataProvider.GameModel.PurchasedExos.Contains(e.captainData.Index))
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

            //    captainImage.sprite = e.captainImage;
            captainName.text = LocTableCache.CommonTable.GetString(e.captainData.NameStringKeyBase);
            captainDescription.text = LocTableCache.CommonTable.GetString(e.captainData.DescriptionKeyBase);
            captainPrice.text = e.captainData.CaptainCost.ToString();
        }

        private void OnDestroy()
        {
            captainDataChanged -= CaptainDataChanged;
            onCaptainItemClick -= OnCaptainItemClick;
        }
        public void GotoBlackMarket()
        {
            GetComponentInParent<ShopPanelScreenController>().GotoBlackMarket();
        }
    }



    public class CaptainDataEventArgs : EventArgs
    {
        public ICaptainData captainData { get; set; }
        public Sprite captainImage { get; set; }
    }
}

