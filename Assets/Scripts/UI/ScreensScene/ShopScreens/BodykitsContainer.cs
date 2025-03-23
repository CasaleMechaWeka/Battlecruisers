using BattleCruisers.Data;
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
    public class BodykitsContainer : MonoBehaviour
    {
        public Image bodykitImage;
        public Image bodykitPreviewImage;
        public Text bodykitName;
        public Text bodykitDescription;
        public Text bodykitPrice;
        public EventHandler<BodykitDataEventArgs> bodykitDataChanged;
        public EventHandler<BodykitDataEventArgs> onBodykitItemClick;
        public BodykitItemController currentItem;
        public IBodykitData currentBodykitData;
        public GameObject btnBuy, ownFeedback;
        public GameObject priceLabel;
        public CanvasGroupButton premiumButton;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private PrefabFactory _prefabFactory;
        public GameObject content;
        public GameObject bodykitMessagePanel;
        public GameObject itemDetailsPanel;
        public Text t_bodykitsMessage;
        public Animator skyAnimator;
        public Animator seaAnimator;
        public GameObject previewCanvas;

        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, PrefabFactory prefabFactory)
        {
            bodykitDataChanged += BodykitDataChanged;
            onBodykitItemClick += OnBodykitItemClick;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            premiumButton.Initialise(_soundPlayer, ScreensSceneGod.Instance.ShowPremiumEditionIAP);
            priceLabel = bodykitPrice.transform.parent.gameObject;
        }

        public void OnEnable()
        {
            if (_dataProvider.GameModel.PurchasedBodykits.Contains(0))
            {
                priceLabel.SetActive(false);
                premiumButton.gameObject.SetActive(false);
            }
            else
            {
                btnBuy.transform.parent.gameObject.SetActive(false);
                premiumButton.gameObject.SetActive(true);
            }
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
                            _dataProvider.GameModel.AddBodykit(currentBodykitData.Index);
                            _dataProvider.SaveGame();
                            await _dataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("BodykitPurchased") + " " + LocTableFactory.CommonTable.GetString(currentBodykitData.NameStringKeyBase));
                            priceLabel.SetActive(false);
                        }
                        else
                        {
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("TryAgain"));
                        }
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                    }
                    catch
                    {
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("TryAgain"));
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
                        _dataProvider.GameModel.AddBodykit(currentBodykitData.Index);
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("BodykitPurchased") + " " + LocTableFactory.CommonTable.GetString(currentBodykitData.NameStringKeyBase));
                        priceLabel.SetActive(false);

                        // Subtract from local economy:
                        _dataProvider.GameModel.Coins -= currentBodykitData.BodykitCost;
                        PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);

                        // Keep track of transaction for later:
                        _dataProvider.GameModel.CoinsChange -= currentBodykitData.BodykitCost;
                        BodykitData bodykit = _dataProvider.StaticData.Bodykits[currentBodykitData.Index];
                        if (_dataProvider.GameModel.OutstandingBodykitTransactions == null)
                        {
                            _dataProvider.GameModel.OutstandingBodykitTransactions = new List<BodykitData>();
                        }
                        _dataProvider.GameModel.OutstandingBodykitTransactions.Add(bodykit);
                        _dataProvider.SaveGame();
                    }
                    catch
                    {
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("TryAgain"));
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
                ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("InsufficientCoins"), null, null);
#else
        // Execute the original line for non-Windows builds
        ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("InsufficientCoins"), GotoBlackMarket, LocTableFactory.ScreensSceneTable.GetString("GetCoins"));
#endif
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

            // Randomize animations
            RandomizeAnimations();
        }

        private void RandomizeAnimations()
        {
            // Generate a random seed
            int randomSeed = UnityEngine.Random.Range(0, int.MaxValue);

            // Set the same seed for both animations
            UnityEngine.Random.InitState(randomSeed);

            // Randomize start positions
            float randomStartPoint = UnityEngine.Random.Range(0f, 1f);

            // Play animations from the random start point
            if (skyAnimator != null)
            {
                skyAnimator.Play(0, -1, randomStartPoint);
            }

            if (seaAnimator != null)
            {
                seaAnimator.Play(0, -1, randomStartPoint);
            }
        }

        private void BodykitDataChanged(object sender, BodykitDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem = (BodykitItemController)sender;
            currentBodykitData = e.bodykitData;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
            btnBuy.transform.parent.gameObject.SetActive(true);
            premiumButton.gameObject.SetActive(false);

            if (_dataProvider.GameModel.PurchasedBodykits.Contains(e.bodykitData.Index))
            {
                btnBuy.SetActive(false);
                priceLabel.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else if (!e.purchasable || e.bodykitData.Index == 0)
            {
                ownFeedback.SetActive(false);
                btnBuy.SetActive(false);
                if (e.bodykitData.Index == 0)
                {
                    btnBuy.transform.parent.gameObject.SetActive(false);
                    priceLabel.SetActive(false);
                    premiumButton.gameObject.SetActive(true);
                }
                else
                    priceLabel.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                priceLabel.SetActive(true);
                ownFeedback.SetActive(false);
            }

            bodykitPrice.text = e.bodykitData.BodykitCost.ToString();
            bodykitImage.sprite = e.bodykitImage;
            bodykitPreviewImage.sprite = e.bodykitImage;
            bodykitName.text = LocTableFactory.CommonTable.GetString(e.bodykitData.NameStringKeyBase);
            bodykitDescription.text = LocTableFactory.CommonTable.GetString(e.bodykitData.DescriptionKeyBase);
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

        private void Start()
        {
            // Ensure the preview canvas is initially inactive
            if (previewCanvas != null)
            {
                previewCanvas.SetActive(false);
            }
        }

        public void OnBodykitPreviewButtonClicked()
        {
            // Activate the preview canvas
            if (previewCanvas != null)
            {
                previewCanvas.SetActive(true);
            }

            // Randomize and restart animations
            RandomizeAndRestartAnimations();
        }

        private void RandomizeAndRestartAnimations()
        {
            // Generate a random start point within the animation
            float randomStartPoint = UnityEngine.Random.Range(0f, 1f);

            // Restart the animations at the random start point
            if (skyAnimator != null)
            {
                skyAnimator.Play(0, -1, randomStartPoint);
            }

            if (seaAnimator != null)
            {
                seaAnimator.Play(0, -1, randomStartPoint);
            }
        }
    }

    public class BodykitDataEventArgs : EventArgs
    {
        public IBodykitData bodykitData { get; set; }
        public Sprite bodykitImage { get; set; }
        public bool purchasable { get; set; }
    }
}

