using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using NSubstitute.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Analytics;
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
        private ILocTable commonStrings;
        public CaptainItemController currentItem;
        private ICaptainData currentCaptainData;
        public List<GameObject> visualOfCaptains = new List<GameObject>();
        public GameObject btnBuy, ownFeedback;

        private string firstNameString;
        private string firstDescrtiptionString;

        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        public GameObject content;
        private ILocTable screensSceneTable;
        public GameObject captainMessagePanel;
        public GameObject itemDetailsPanel;

        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            captainDataChanged += CaptainDataChanged;
            onCaptainItemClick += OnCaptainItemClick;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            firstNameString = captainName.text;
            firstDescrtiptionString = captainDescription.text;
            screensSceneTable = LandingSceneGod.Instance.screenSceneStrings;
        }

        private async void OnEnable()
        {
            if (firstNameString != null)
                captainName.text = firstNameString;
            if (firstNameString != firstDescrtiptionString)
                captainDescription.text = firstDescrtiptionString;
        }

        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if (_dataProvider.GameModel.Coins >= currentCaptainData.CaptainCost)
            {
                if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                {
                    // Online purchasing
                    try
                    {
                        bool result = await _dataProvider.PurchaseCaptainV2(currentCaptainData.Index);
                        if (result)
                        {
                        //    await _dataProvider.SyncCurrencyFromCloud();
                            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            _dataProvider.GameModel.Captains[currentCaptainData.Index].isOwned = true;
                            _dataProvider.GameModel.AddExo(currentCaptainData.Index);
                            _dataProvider.SaveGame();
                            await _dataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("CaptainExoPurchased") + " " + commonStrings.GetString(currentCaptainData.NameStringKeyBase));
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

                    string logName = currentCaptainData.NameStringKeyBase;
#if LOG_ANALYTICS
                Debug.Log("Analytics: " + logName);
#endif
                    IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
                    Dictionary<string, object> transactionDetails = new Dictionary<string, object>() { { "exoIndex", currentCaptainData.Index } };
                    try
                    {
                        AnalyticsService.Instance.CustomData("Shop_Exo_Bought", transactionDetails);
                        AnalyticsService.Instance.Flush();
                    }
                    catch (ConsentCheckException ex)
                    {
                        Debug.Log(ex.Message);
                    }
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
                        _dataProvider.GameModel.Captains[currentCaptainData.Index].isOwned = true;
                        _dataProvider.GameModel.AddExo(currentCaptainData.Index);
                        _dataProvider.SaveGame();
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("CaptainExoPurchased") + " " + commonStrings.GetString(currentCaptainData.NameStringKeyBase));

                        // Subtract from local economy:
                        _dataProvider.GameModel.Coins -= currentCaptainData.CaptainCost;
                        PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);

                        // Keep track of transaction for later:
                        _dataProvider.GameModel.CoinsChange -= currentCaptainData.CaptainCost;
                        CaptainData captain = _dataProvider.GameModel.Captains[currentCaptainData.Index];
                        if (_dataProvider.GameModel.OutstandingCaptainTransactions == null)
                        {
                            _dataProvider.GameModel.OutstandingCaptainTransactions = new List<CaptainData>();
                        }
                        _dataProvider.GameModel.OutstandingCaptainTransactions.Add(captain);
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
            if (e.captainData.IsOwned)
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

