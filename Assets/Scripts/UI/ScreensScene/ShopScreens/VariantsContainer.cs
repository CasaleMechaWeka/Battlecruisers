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
using System.Collections;
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
        public ILocTable commonStrings;
        private ILocTable screensSceneTable;
        public VariantItemController currentItem;
        public IVariantData currentVariantData;
        public GameObject btnBuy, ownFeedback;

        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private VariantPrefab currentVariant;
        public GameObject content;


        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            screensSceneTable = LandingSceneGod.Instance.screenSceneStrings;
            variantDataChanged += VariantDataChanged;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            buildingStatsController.Initialise();
            unitStatsController.Initialise();
        }

        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if(_dataProvider.GameModel.Credits >=  currentVariantData.VariantCredits)
            {
                if(await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                {
                    // online purchase
                    try
                    {
                        bool result = await _dataProvider.PurchaseVariant(currentVariantData.Index);
                        if(result)
                        {
                            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._clickedFeedbackVariantImage.color = new Color(currentItem._clickedFeedbackVariantImage.color.r, currentItem._clickedFeedbackVariantImage.color.g, currentItem._clickedFeedbackVariantImage.color.b, 1f);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            _dataProvider.GameModel.Variants[currentVariantData.Index].isOwned = true;
                            _dataProvider.GameModel.AddVariant(currentVariantData.Index);
                            _dataProvider.SaveGame();
                            await _dataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("VariantPurchased") + " " + commonStrings.GetString(currentVariantData.VariantNameStringKeyBase));
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
                    // offline purchase
                    try
                    {

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
                ScreensSceneGod.Instance.messageBox.ShowMessage(screensSceneTable.GetString("InsufficientCredits"), null, null);
                return;
            }
        }

        private void VariantDataChanged(object sender, VariantDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem._clickedFeedbackVariantImage.color = new Color(currentItem._clickedFeedbackVariantImage.color.r, currentItem._clickedFeedbackVariantImage.color.g, currentItem._clickedFeedbackVariantImage.color.b, 64f/255);
            currentItem = (VariantItemController)sender;
            currentVariantData = e.variantData;
            currentVariant = e.varint;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");

            if(e.variantData.IsOwned)
            {
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }

            if(currentVariant.IsUnit())
            {
                buildingStatsController.gameObject.SetActive(false);
                unitStatsController.gameObject.SetActive(true);
                unitStatsController.ShowStatsOfVariant(currentVariant.GetUnit(), currentVariant);
            }
            else
            {
                buildingStatsController.gameObject.SetActive(true);
                unitStatsController.gameObject.SetActive(false);
                buildingStatsController.ShowStatsOfVariant(currentVariant.GetBuilding(), currentVariant);
            }

            VariantPrice.text = e.variantData.VariantCredits.ToString();
            ParentImage.sprite = e.parentSprite;
            variantIcon.sprite = e.variantSprite;
            VariantName.text = commonStrings.GetString(e.variantData.VariantNameStringKeyBase);
            variantDescription.text = commonStrings.GetString(e.variantData.VariantDescriptionStringKeyBase);
            ParentName.text = e.parentName;
        }

        private void OnDestroy()
        {
            variantDataChanged -= VariantDataChanged;
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

