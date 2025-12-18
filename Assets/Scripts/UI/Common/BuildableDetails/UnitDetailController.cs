using BattleCruisers.Buildables.Units;
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
    public class UnitDetailController : MonoBehaviour
    {
        private int _selectedVariant;
        public CanvasGroupButton leftNav, rightNav;
        private IUnit _selectedUnit;
        public IUnit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                Debug.Log($"UnitDetailController.SelectedUnit SET: old={(_selectedUnit == null ? "NULL" : _selectedUnit.ToString())}, new={(value == null ? "NULL" : value.ToString())}");
                _selectedUnit = value;
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
        private Dictionary<IUnit, List<int>> _unlockedVariants;
        private int _index;

        public Image variantIcon;
        public Text variantName;
        public Text variantDescription;
        public Text variantParentName;
        public StatsController<IUnit> variantStats;

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

            CollectAllUnitVariants();

            if (buyButton != null)
                buyButton.Initialise(soundPlayer, BuyVariant);
        }
        private void SetInitVariant()
        {
            _selectedVariant = DataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_selectedUnit);
            if (_unlockedVariants != null && _unlockedVariants.ContainsKey(_selectedUnit))
            {
                if (_unlockedVariants[_selectedUnit].Count > 0)
                {
                    if (_unlockedVariants[_selectedUnit].IndexOf(_selectedVariant) == 0)
                    {
                        if (_unlockedVariants[_selectedUnit].Count == 1)
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
                        ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
                        return;
                    }
                    else if (_unlockedVariants[_selectedUnit].IndexOf(_selectedVariant) == _unlockedVariants[_selectedUnit].Count - 1)
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(false);
                        _index = _unlockedVariants[_selectedUnit].Count - 1;
                        ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
                        return;
                    }
                    else if (_unlockedVariants[_selectedUnit].IndexOf(_selectedVariant) < 0)
                    {
                        leftNav.gameObject.SetActive(false);
                        rightNav.gameObject.SetActive(true);
                        _index = -1;
                        ShowOriginalUnit();
                        return;
                    }
                    else
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(true);
                        _index = _unlockedVariants[_selectedUnit].IndexOf(_selectedVariant);
                        ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
                        return;
                    }
                }
                else
                {
                    leftNav.gameObject.SetActive(false);
                    rightNav.gameObject.SetActive(false);
                    _index = -1;
                    ShowOriginalUnit();
                }
            }
            else
            {
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(false);
                _index = -1;
                ShowOriginalUnit();
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
                variantStats.ShowStatsOfVariant(_selectedUnit, variant);

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
        private void ShowOriginalUnit()
        {
            SetPurchaseMode(false);
            variantIcon.gameObject.SetActive(false);
            variantName.gameObject.SetActive(false);
            GetComponent<ComparableUnitDetailsController>().ShowItemDetails();
        }
        private void LeftNavButton_OnClicked()
        {
            --_index;
            int current_index = DataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_selectedUnit);
            if (_index <= -1)
            {
                _index = -1;
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(true);
                ShowOriginalUnit();
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
            DataProvider.GameModel.PlayerLoadout.AddSelectedVariant(_unlockedVariants[_selectedUnit][_index]);
            DataProvider.SaveGame();
            ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _unlockedVariants[_selectedUnit][_index] });
        }
        private void RightNavButton_OnClicked()
        {
            ++_index;
            int current_index = DataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_selectedUnit);
            if (_index >= _unlockedVariants[_selectedUnit].Count - 1)
            {
                _index = _unlockedVariants[_selectedUnit].Count - 1;
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(false);
            }
            else
            {
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(true);
            }
            DataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
            DataProvider.GameModel.PlayerLoadout.AddSelectedVariant(_unlockedVariants[_selectedUnit][_index]);
            DataProvider.SaveGame();
            ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _unlockedVariants[_selectedUnit][_index] });
        }
        public void CollectAllUnitVariants()
        {
            _unlockedVariants = new Dictionary<IUnit, List<int>>();

            for (int i = 0; i < StaticData.Variants.Count; i++)
            {
                VariantPrefab variantPrefab = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(i));
                if (variantPrefab != null && variantPrefab.IsUnit())
                {
                    IUnit unit = variantPrefab.GetUnit();
                    if (_unlockedVariants.ContainsKey(unit))
                        _unlockedVariants[unit].Add(i);
                    else
                        _unlockedVariants[unit] = new List<int> { i };
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
            Debug.Log($"BuyVariant called. _selectedUnit: {(_selectedUnit == null ? "NULL" : _selectedUnit.ToString())}, _index: {_index}");

            if (_selectedUnit == null || !_unlockedVariants.ContainsKey(_selectedUnit))
            {
                Debug.LogError($"BuyVariant early exit: _selectedUnit null={_selectedUnit == null}, containsKey={(_selectedUnit != null && _unlockedVariants.ContainsKey(_selectedUnit))}");
                ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("TryAgain"));
                return;
            }

            int variantId = _unlockedVariants[_selectedUnit][_index];
            Debug.Log($"BuyVariant processing variantId: {variantId}");

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
                                Debug.LogError("BuyVariant: PurchaseVariant returned false");
                                ScreensSceneGod.Instance.messageBox.ShowMessage(
                    LocTableCache.ScreensSceneTable.GetString("TryAgain"));
                            }
                        }
                        catch
                        {
                            Debug.LogError("BuyVariant: Exception during purchase");
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
                    Debug.LogWarning($"BuyVariant: Insufficient credits. Has: {DataProvider.GameModel.Credits}, Needs: {variantData.VariantCredits}");
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
