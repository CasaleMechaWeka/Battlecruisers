using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
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
        public EventHandler<BodykitDataEventArgs> onBodykitItemClick;
        private ILocTable commonStrings;
        public BodykitItemController currentItem;
        public IBodykitData currentBodykitData;
        public GameObject btnBuy, ownFeedback;

        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        public GameObject content;
        private ILocTable screensSceneTable;
        public GameObject bodykitMessagePanel;
        public GameObject itemDetailsPanel;

        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            bodykitDataChanged += BodykitDataChanged;
            onBodykitItemClick += OnBodykitItemClick;
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
                    try
                    {
                        bool result = await _dataProvider.PurchaseBodykitV2(currentBodykitData.Index);
                        if (result)
                        {
                        //    await _dataProvider.SyncCurrencyFromCloud();
                            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            _dataProvider.GameModel.Bodykits[currentBodykitData.Index].isOwned = true;
                            _dataProvider.GameModel.AddBodykit(currentBodykitData.Index);
                            _dataProvider.SaveGame();
                            await _dataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("BodykitPurchased") + " " + commonStrings.GetString(currentBodykitData.NameStringKeyBase));
                        }
                        else
                        {
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("TryAgain"));
                        }
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                    }
                    catch
                    {
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("TryAgain"));
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
                        _dataProvider.GameModel.Bodykits[currentBodykitData.Index].isOwned = true;
                        _dataProvider.GameModel.AddBodykit(currentBodykitData.Index);
                        _dataProvider.SaveGame();
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("BodykitPurchased") + " " + commonStrings.GetString(currentBodykitData.NameStringKeyBase));

                        // Subtract from local economy:
                        _dataProvider.GameModel.Coins -= currentBodykitData.BodykitCost;
                        PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);

                        // Keep track of transaction for later:
                        _dataProvider.GameModel.CoinsChange -= currentBodykitData.BodykitCost;
                        BodykitData bodykit = _dataProvider.GameModel.Bodykits[currentBodykitData.Index];
                        if (_dataProvider.GameModel.OutstandingBodykitTransactions == null)
                        {
                            _dataProvider.GameModel.OutstandingBodykitTransactions = new List<BodykitData>();
                        }
                        _dataProvider.GameModel.OutstandingBodykitTransactions.Add(bodykit);
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

        private void OnBodykitItemClick(object sender, BodykitDataEventArgs e)
        {
            // Hide the message panel and show the item details
            if (bodykitMessagePanel.activeSelf)
            {
                bodykitMessagePanel.SetActive(false);
                itemDetailsPanel.SetActive(true);
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
            onBodykitItemClick -= OnBodykitItemClick;
        }

        public void GotoBlackMarket()
        {
            GetComponentInParent<ShopPanelScreenController>().GotoBlackMarket();
        }
    }

    public class BodykitDataEventArgs : EventArgs
    {
        public IBodykitData bodykitData { get; set; }
        public Sprite bodykitImage { get; set; }
    }
}

