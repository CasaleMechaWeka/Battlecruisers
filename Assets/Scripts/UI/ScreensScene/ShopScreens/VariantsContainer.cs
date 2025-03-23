using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
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
    public class VariantsContainer : MonoBehaviour
    {
        public Image ParentImage;
        public Text ParentName;
        public Text VariantName;
        public Image variantIcon;
        public Text variantDescription;
        public Text VariantPrice;
        public StatsController<IBuilding> buildingStatsController;
        public StatsController<IUnit> unitStatsController;

        public EventHandler<VariantDataEventArgs> variantDataChanged;
        public EventHandler<VariantDataEventArgs> onVariantItemClick;
        public VariantItemController currentItem;
        public IVariantData currentVariantData;
        public GameObject btnBuy, ownFeedback;

        public GameObject priceLabel;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private PrefabFactory _prefabFactory;
        private VariantPrefab currentVariant;
        public GameObject content;
        public GameObject variantMessagePanel;
        public GameObject itemDetailsPanel;
        public Text t_variantsMessage;

        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, PrefabFactory prefabFactory)
        {
            variantDataChanged += VariantDataChanged;
            onVariantItemClick += OnVariantItemClick;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            buildingStatsController.Initialise();
            unitStatsController.Initialise();
            priceLabel = VariantPrice.transform.parent.gameObject;
        }

        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if (_dataProvider.GameModel.Credits >= currentVariantData.VariantCredits)
            {
                if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                {
                    // online purchase
                    try
                    {
                        bool result = await _dataProvider.PurchaseVariant(currentVariantData.Index);
                        if (result)
                        {
                            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._clickedFeedbackVariantImage.color = new Color(currentItem._clickedFeedbackVariantImage.color.r, currentItem._clickedFeedbackVariantImage.color.g, currentItem._clickedFeedbackVariantImage.color.b, 1f);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            _dataProvider.GameModel.AddVariant(currentVariantData.Index);
                            _dataProvider.SaveGame();
                            await _dataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("PurchasedVariant") + " " + LocTableFactory.CommonTable.GetString(currentVariantData.VariantNameStringKeyBase));
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
                    // offline purchase
                    try
                    {
                        PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                        currentItem._clickedFeedback.SetActive(true);
                        currentItem._clickedFeedbackVariantImage.color = new Color(currentItem._clickedFeedbackVariantImage.color.r, currentItem._clickedFeedbackVariantImage.color.g, currentItem._clickedFeedbackVariantImage.color.b, 1f);
                        currentItem._ownedItemMark.SetActive(true);
                        btnBuy.SetActive(false);
                        ownFeedback.SetActive(true);
                        ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                        _dataProvider.GameModel.AddVariant(currentVariantData.Index);
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("PurchasedVariant") + " " + LocTableFactory.CommonTable.GetString(currentVariantData.VariantNameStringKeyBase));
                        priceLabel.SetActive(false);

                        // Subtract from local economy:
                        _dataProvider.GameModel.Credits -= currentVariantData.VariantCredits;
                        PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);

                        // Keep track of transaction for later:
                        _dataProvider.GameModel.CreditsChange -= currentVariantData.VariantCredits;
                        VariantData variant = _dataProvider.StaticData.Variants[currentVariantData.Index];
                        if (_dataProvider.GameModel.OutstandingVariantTransactions == null)
                        {
                            _dataProvider.GameModel.OutstandingVariantTransactions = new List<VariantData>();
                        }
                        _dataProvider.GameModel.OutstandingVariantTransactions.Add(variant);
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
                ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableFactory.ScreensSceneTable.GetString("InsufficientCredits"), null, null);
                return;
            }
        }

        private void OnVariantItemClick(object sender, VariantDataEventArgs e)
        {
            // Hide the message panel and show the item details
            if (variantMessagePanel.activeSelf)
            {
                variantMessagePanel.SetActive(false);
                itemDetailsPanel.SetActive(true);
            }
        }

        private void VariantDataChanged(object sender, VariantDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem._clickedFeedbackVariantImage.color = new Color(currentItem._clickedFeedbackVariantImage.color.r, currentItem._clickedFeedbackVariantImage.color.g, currentItem._clickedFeedbackVariantImage.color.b, 64f / 255);
            currentItem = (VariantItemController)sender;
            currentVariantData = e.variantData;
            currentVariant = e.varint;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");

            if (_dataProvider.GameModel.PurchasedVariants.Contains(e.variantData.Index))
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

            if (currentVariant.IsUnit())
            {
                buildingStatsController.gameObject.SetActive(false);
                unitStatsController.gameObject.SetActive(true);
                unitStatsController.ShowStatsOfVariant(currentVariant.GetUnit(ScreensSceneGod.Instance._prefabFactory), currentVariant);
            }
            else
            {
                buildingStatsController.gameObject.SetActive(true);
                unitStatsController.gameObject.SetActive(false);
                buildingStatsController.ShowStatsOfVariant(currentVariant.GetBuilding(ScreensSceneGod.Instance._prefabFactory), currentVariant);
            }

            VariantPrice.text = e.variantData.VariantCredits.ToString();
            ParentImage.sprite = e.parentSprite;
            variantIcon.sprite = e.variantSprite;
            VariantName.text = LocTableFactory.CommonTable.GetString(e.variantData.VariantNameStringKeyBase);
            variantDescription.text = LocTableFactory.CommonTable.GetString(e.variantData.VariantDescriptionStringKeyBase);
            ParentName.text = e.parentName;
        }

        private void OnDestroy()
        {
            variantDataChanged -= VariantDataChanged;
            onVariantItemClick -= OnVariantItemClick;
        }
        public void GotoBlackMarket()
        {
            GetComponentInParent<ShopPanelScreenController>().GotoBlackMarket();
        }
    }

    public class VariantDataEventArgs : EventArgs
    {
        public IVariantData variantData { get; set; }
        public Sprite parentSprite { get; set; }
        public Sprite variantSprite { get; set; }
        public string parentName { get; set; }
        public VariantPrefab varint { get; set; }
    }
}

