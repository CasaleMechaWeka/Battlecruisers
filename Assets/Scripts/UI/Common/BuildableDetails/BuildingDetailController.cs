using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BuildingDetailController : MonoBehaviour
    {
        private int _selectedVariant;
        public CanvasGroupButton leftNav, rightNav;
        private IBuilding _selectedBuilding;
        public IBuilding SelectedBuilding
        {
            get { return _selectedBuilding; }
            set
            {
                _selectedBuilding = value;
                SetInitVariant();
            }
        }

        private ItemButton _currentButton;
        public ItemButton currentButton
        {
            get => _currentButton;
            set { _currentButton = value; }
        }

        private SingleSoundPlayer _soundPlayer;
        private Dictionary<IBuilding, List<int>> _unlockedVariants;
        private int _index;

        public Image variantIcon;
        public Text variantName;
        public Text variantDescription;
        public Text variantParentName;
        public StatsController<IBuilding> variantStats;
        [SerializeField] GameObject purchasingPanel;
        [SerializeField] Text variantCostText;
        [SerializeField] Text creditsBalanceText;
        [SerializeField] GameObject tooExpensiveOverlay;
        [SerializeField] CanvasGroupButton buyButton;
        private int selectedVariant;

        public event Action<bool> PurchaseModeToggled;
        public void Initialize(SingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(soundPlayer);
            Helper.AssertIsNotNull(leftNav, rightNav);
            _soundPlayer = soundPlayer;

            leftNav.Initialise(_soundPlayer, LeftNavButton_OnClicked);
            rightNav.Initialise(_soundPlayer, RightNavButton_OnClicked);

            variantIcon.gameObject.SetActive(false);
            variantName.gameObject.SetActive(false);

            CollectAllBuildingVariants();

            if (buyButton != null)
                buyButton.Initialise(soundPlayer, BuyVariant);
        }
        private void SetInitVariant()
        {
            _selectedVariant = DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_selectedBuilding);
            if (_unlockedVariants != null && _unlockedVariants.ContainsKey(_selectedBuilding))
            {
                if (_unlockedVariants[_selectedBuilding].Count > 0)
                {
                    if (_unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant) == 0)
                    {
                        if (_unlockedVariants[_selectedBuilding].Count == 1)
                        {
                            leftNav.gameObject.SetActive(true);
                            rightNav.gameObject.SetActive(false);
                        }
                        else
                        {
                            leftNav.gameObject.SetActive(true);
                            rightNav.gameObject.SetActive(true);
                        }
                        _index = 0;
                        ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
                        return;
                    }
                    else if (_unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant) == _unlockedVariants[_selectedBuilding].Count - 1)
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(false);
                        _index = _unlockedVariants[_selectedBuilding].Count - 1;
                        ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
                        return;
                    }
                    else if (_unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant) < 0)
                    {
                        leftNav.gameObject.SetActive(false);
                        rightNav.gameObject.SetActive(true);
                        _index = -1;
                        ShowOriginalBuilding();
                        return;
                    }
                    else
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(true);
                        _index = _unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant);
                        ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
                        return;
                    }
                }
                else
                {
                    leftNav.gameObject.SetActive(false);
                    rightNav.gameObject.SetActive(false);
                    _index = -1;
                    ShowOriginalBuilding();
                }
            }
            else
            {
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(false);
                _index = -1;
                ShowOriginalBuilding();
            }
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _selectedVariant });
        }
        private void ShowVariantDetail(int index)
        {
            if (index < 0)
                return;
            if (variantIcon != null)
                variantIcon.gameObject.SetActive(true);
            if (variantName != null)
                variantName.gameObject.SetActive(true);
            VariantPrefab variant = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
            if (variant == null)
                return;
            if (variantName != null)
                variantName.text = LocTableCache.CommonTable.GetString(StaticData.Variants[index].VariantNameStringKeyBase);
            if (variantIcon != null)
                variantIcon.sprite = variant.variantSprite;
            if (variantParentName != null)
                variantParentName.text = variant.GetParentName();
            if (variantDescription != null)
                variantDescription.text = LocTableCache.CommonTable.GetString(StaticData.Variants[index].VariantDescriptionStringKeyBase);
            if (variantStats != null)
                variantStats.ShowStatsOfVariant(_selectedBuilding, variant);

            selectedVariant = index;

            if (DataProvider.GameModel.PurchasedVariants.Contains(index))
            {
                HidePurchasingPanel();
            }
            else
            {
                ShowPurchaseInfo(index);
            }
        }
        private void ShowOriginalBuilding()
        {
            SetPurchaseMode(false);
            if (variantIcon != null)
                variantIcon.gameObject.SetActive(false);
            if (variantName != null)
                variantName.gameObject.SetActive(false);
            GetComponent<ComparableBuildingDetailsController>().ShowItemDetails();
        }
        private void LeftNavButton_OnClicked()
        {
            --_index;
            int current_index = DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_selectedBuilding);
            if (_index <= -1)
            {
                _index = -1;
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(true);
                ShowOriginalBuilding();
                DataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
                DataProvider.SaveGame();
                _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = -1 });
                return;
            }
            else
            {
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(true);
            }
            DataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
            DataProvider.GameModel.PlayerLoadout.AddSelectedVariant(_unlockedVariants[_selectedBuilding][_index]);
            DataProvider.SaveGame();
            ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _unlockedVariants[_selectedBuilding][_index] });
        }
        private void RightNavButton_OnClicked()
        {
            ++_index;
            int current_index = DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_selectedBuilding);
            if (_index >= _unlockedVariants[_selectedBuilding].Count - 1)
            {
                _index = _unlockedVariants[_selectedBuilding].Count - 1;
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(false);
            }
            else
            {
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(true);
            }
            DataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
            DataProvider.GameModel.PlayerLoadout.AddSelectedVariant(_unlockedVariants[_selectedBuilding][_index]);
            DataProvider.SaveGame();
            ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _unlockedVariants[_selectedBuilding][_index] });
        }
        public void CollectAllBuildingVariants()
        {
            _unlockedVariants = new Dictionary<IBuilding, List<int>>();

            for (int i = 0; i < StaticData.Variants.Count; i++)
            {
                VariantPrefab variantPrefab = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(i));
                if (variantPrefab != null && !variantPrefab.IsUnit())
                {
                    IBuilding building = variantPrefab.GetBuilding();
                    if (_unlockedVariants.ContainsKey(building))
                        _unlockedVariants[building].Add(i);
                    else
                        _unlockedVariants[building] = new List<int> { i };
                }
            }
        }
        private void SetPurchaseMode(bool showPurchase)
        {
            if (purchasingPanel != null)
                purchasingPanel.SetActive(showPurchase);

            PurchaseModeToggled?.Invoke(showPurchase);
        }

        private async void BuyVariant()
        {
            if (_selectedBuilding == null || !_unlockedVariants.ContainsKey(_selectedBuilding))
            {
                ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("TryAgain"));
                return;
            }

            int variantId = _unlockedVariants[_selectedBuilding][_index];

            await TransactionLocker.ProcessTransaction(variantId, async () =>
            {
                VariantData variantData = StaticData.Variants[variantId];

                if (DataProvider.GameModel.Credits >= variantData.VariantCredits)
                {
                    if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                    {
                        try
                        {
                            bool result = await DataProvider.PurchaseVariant(variantData.Index);
                            if (result)
                            {
                                PlayerInfoPanelController.Instance.UpdateInfo();
                                DataProvider.GameModel.AddVariant(variantData.Index);
                                DataProvider.SaveGame();
                                await DataProvider.CloudSave();
                                ShowVariantDetail(variantData.Index);
                                ScreensSceneGod.Instance.messageBox.ShowMessage(
                    LocTableCache.ScreensSceneTable.GetString("PurchasedVariant") + " " +
                    LocTableCache.CommonTable.GetString(variantData.VariantNameStringKeyBase));
                            }
                            else
                            {
                                ScreensSceneGod.Instance.messageBox.ShowMessage(
                    LocTableCache.ScreensSceneTable.GetString("TryAgain"));
                            }
                        }
                        catch
                        {
                            ScreensSceneGod.Instance.messageBox.ShowMessage(
                LocTableCache.ScreensSceneTable.GetString("TryAgain"));
                        }
                    }
                    else
                    {
                        DataProvider.GameModel.AddVariant(variantData.Index);
                        ShowVariantDetail(variantData.Index);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(
                        LocTableCache.ScreensSceneTable.GetString("PurchasedVariant") + " " +
                        LocTableCache.CommonTable.GetString(variantData.VariantNameStringKeyBase));

                        DataProvider.GameModel.Credits -= variantData.VariantCredits;
                        PlayerInfoPanelController.Instance.UpdateInfo();
                        DataProvider.GameModel.CreditsChange -= variantData.VariantCredits;

                        if (DataProvider.GameModel.OutstandingVariantTransactions == null)
                            DataProvider.GameModel.OutstandingVariantTransactions = new List<VariantData>();

                        DataProvider.GameModel.OutstandingVariantTransactions.Add(variantData);
                        DataProvider.SaveGame();
                    }
                }
                else
                {
                    ScreensSceneGod.Instance.messageBox.ShowMessage(
        LocTableCache.ScreensSceneTable.GetString("InsufficientCredits"), null, null);
                }
            });
        }

        private void ShowPurchaseInfo(int variantIndex)
        {
            int cost = StaticData.Variants[variantIndex].VariantCredits;
            long balance = DataProvider.GameModel.Credits;

            ToggleCurrencyGroups(useCredits: true);

            if (creditsBalanceText != null)
                creditsBalanceText.text = balance.ToString();
            if (variantCostText != null)
                variantCostText.text = cost.ToString();

            SetPurchaseMode(true);

            bool canAffordVariant = cost <= balance;
            if (tooExpensiveOverlay != null)
                tooExpensiveOverlay.SetActive(!canAffordVariant);
            if (buyButton != null)
            {
                CanvasGroup buyCg = buyButton.GetComponent<CanvasGroup>();
                if (buyCg != null)
                    buyCg.interactable = canAffordVariant;
            }
        }

        private void ToggleCurrencyGroups(bool useCredits)
        {
            if (purchasingPanel == null)
                return;
            Transform credits = purchasingPanel.transform.Find("BuyWithCreditsButton");
            Transform coins = purchasingPanel.transform.Find("BuyWithCoinsButton");
            if (credits != null)
                credits.gameObject.SetActive(useCredits);
            if (coins != null)
                coins.gameObject.SetActive(!useCredits);
        }

        private void HidePurchasingPanel()
        {
            SetPurchaseMode(false);
        }
    }
}
