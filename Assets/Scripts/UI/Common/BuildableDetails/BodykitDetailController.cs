using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Properties;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BodykitDetailController : MonoBehaviour
    {
        IBroadcastingProperty<HullKey> _selectedHull;
        private HullType _selectedHullType => StaticPrefabKeys.Hulls.GetHullType(DataProvider.GameModel.PlayerLoadout.Hull);
        private HullType _hullType;
        public HullType hullType
        {
            get { return _hullType; }
            set
            {
                _hullType = value;
                if (_unlockedBodykits.ContainsKey(_hullType))
                {
                    if (_unlockedBodykits[_hullType].Count > 0)
                    {
                        if (_unlockedBodykits[_hullType].IndexOf(DataProvider.GameModel.PlayerLoadout.SelectedBodykit) == 0)
                        {
                            if (_unlockedBodykits[_hullType].Count == 1)
                            {
                                leftNavButton.gameObject.SetActive(true);
                                rightNavButton.gameObject.SetActive(false);
                            }
                            else
                            {
                                leftNavButton.gameObject.SetActive(true);
                                rightNavButton.gameObject.SetActive(true);
                            }
                            _index = 0;
                            ShowBodyKitDetail(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
                            return;
                        }
                        else if (_unlockedBodykits[_hullType].IndexOf(DataProvider.GameModel.PlayerLoadout.SelectedBodykit) == _unlockedBodykits[_hullType].Count - 1)
                        {
                            leftNavButton.gameObject.SetActive(true);
                            rightNavButton.gameObject.SetActive(false);
                            _index = _unlockedBodykits[_hullType].Count - 1;
                            ShowBodyKitDetail(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
                            return;
                        }
                        else if (_unlockedBodykits[_hullType].IndexOf(DataProvider.GameModel.PlayerLoadout.SelectedBodykit) < 0)
                        {
                            leftNavButton.gameObject.SetActive(false);
                            rightNavButton.gameObject.SetActive(true);
                            _index = -1;
                            ShowOriginCruiser();
                            return;
                        }
                        else
                        {
                            leftNavButton.gameObject.SetActive(true);
                            rightNavButton.gameObject.SetActive(true);
                            _index = _unlockedBodykits[_hullType].IndexOf(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
                            ShowBodyKitDetail(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
                            return;
                        }
                    }
                    else
                    {
                        leftNavButton.gameObject.SetActive(false);
                        rightNavButton.gameObject.SetActive(false);
                        _index = -1;
                        ShowOriginCruiser();
                    }
                }
                else
                {
                    leftNavButton.gameObject.SetActive(false);
                    rightNavButton.gameObject.SetActive(false);
                    _index = -1;
                    ShowOriginCruiser();
                }
            }
        }

        private SingleSoundPlayer _soundPlayer;
        private Dictionary<HullType, List<int>> _unlockedBodykits = new Dictionary<HullType, List<int>>();
        private int _index;
        public CanvasGroupButton leftNavButton, rightNavButton;

        [SerializeField] GameObject purchasingPanel;
        [SerializeField] Text bodykitCostText;
        [SerializeField] Text coinBalanceText;
        [SerializeField] GameObject tooExpensiveOverlay;
        [SerializeField] CanvasGroupButton buyButton;
        [SerializeField] GameObject selectorPanel;
        [SerializeField] SelectCruiserButton selectCruiserButton;

        public event Action<bool> PurchaseModeToggled;

        private int selectedBodykit;
        public void Initialise(SingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(soundPlayer);
            Helper.AssertIsNotNull(leftNavButton, rightNavButton);
            _soundPlayer = soundPlayer;

            leftNavButton.Initialise(_soundPlayer, LeftNavButton_OnClicked);
            rightNavButton.Initialise(_soundPlayer, RightNavButton_OnClicked);
            CollectAllBodykits();

            if (buyButton != null)
                buyButton.Initialise(soundPlayer, BuyBodykit);
        }
        public void RegisterSelectedHull(IBroadcastingProperty<HullKey> selectedHull)
        {
            _selectedHull = selectedHull;
            _selectedHull.ValueChanged += OnSelectedNewHullType;
        }

        private void OnSelectedNewHullType(object sender, EventArgs e)
        {
            if (_index == -1)
            {
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = -1;
            }
            else
            {
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = _unlockedBodykits[_hullType][_index];
            }
            DataProvider.SaveGame();
        }
        private void LeftNavButton_OnClicked()
        {
            --_index;
            if (_index <= -1)
            {
                _index = -1;
                leftNavButton.gameObject.SetActive(false);
                rightNavButton.gameObject.SetActive(true);
                if (_hullType == _selectedHullType)
                {
                    DataProvider.GameModel.PlayerLoadout.SelectedBodykit = -1;
                    DataProvider.SaveGame();
                }
                ShowOriginCruiser();
                return;
            }
            else
            {
                leftNavButton.gameObject.SetActive(true);
                rightNavButton.gameObject.SetActive(true);
            }
            if (_hullType == _selectedHullType)
            {
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = _unlockedBodykits[_hullType][_index];
                DataProvider.SaveGame();
            }
            ShowBodyKitDetail(_unlockedBodykits[_hullType][_index]);
        }

        private void RightNavButton_OnClicked()
        {
            ++_index;
            if (_index >= _unlockedBodykits[_hullType].Count - 1)
            {
                _index = _unlockedBodykits[_hullType].Count - 1;
                leftNavButton.gameObject.SetActive(true);
                rightNavButton.gameObject.SetActive(false);
            }
            else
            {
                leftNavButton.gameObject.SetActive(true);
                rightNavButton.gameObject.SetActive(true);
            }
            if (_hullType == _selectedHullType)
            {
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = _unlockedBodykits[_hullType][_index];
                DataProvider.SaveGame();
            }
            ShowBodyKitDetail(_unlockedBodykits[_hullType][_index]);
        }
        private void ShowBodyKitDetail(int index)
        {
            if (index < 0)
                return;
            Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(index));
            GetComponent<ComparableCruiserDetailsController>().itemName.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[index].NameStringKeyBase);
            GetComponent<ComparableCruiserDetailsController>().itemDescription.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[index].DescriptionKeyBase);
            GetComponent<ComparableCruiserDetailsController>().itemImage.sprite = bodykit.BodykitImage;

            selectedBodykit = index;

            if (DataProvider.GameModel.PurchasedBodykits.Contains(index))
            {
                HidePurchasingPanel();
            }
            else
            {
                ShowPurchaseInfo(index);
            }
        }
        private void ShowOriginCruiser()
        {
            GetComponent<ComparableCruiserDetailsController>().ShowItemDetails();
        }
        public void CollectAllBodykits()
        {
            _unlockedBodykits = new Dictionary<HullType, List<int>>();
            for (int i = 0; i < StaticData.Bodykits.Count; i++)
            {
                // Now collecting ALL bodykits, not just owned
                Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(i));
                if (_unlockedBodykits.ContainsKey(bodykit.cruiserType))
                {
                    _unlockedBodykits[bodykit.cruiserType].Add(i);
                }
                else
                {
                    _unlockedBodykits[bodykit.cruiserType] = new List<int> { i };
                }
            }
        }
        private void SetPurchaseMode(bool showPurchase)
        {
            if (purchasingPanel != null)
                purchasingPanel.SetActive(showPurchase);

            PurchaseModeToggled?.Invoke(showPurchase);
        }

        void ShowPurchaseInfo(int bodykitIndex)
        {
            int cost = StaticData.Bodykits[bodykitIndex].BodykitCost;
            long balance = DataProvider.GameModel.Coins;

            ToggleCurrencyGroups(useCredits: false);

            if (coinBalanceText != null)
            {
                coinBalanceText.text = balance.ToString();
                Transform balanceRow = coinBalanceText.transform.parent;
                if (balanceRow != null)
                    balanceRow.gameObject.SetActive(true);
                coinBalanceText.gameObject.SetActive(true);
            }
            if (bodykitCostText != null)
            {
                bodykitCostText.text = cost.ToString();
                Transform costRow = bodykitCostText.transform.parent;
                if (costRow != null)
                    costRow.gameObject.SetActive(true);
                bodykitCostText.gameObject.SetActive(true);
            }

            if (purchasingPanel != null)
                purchasingPanel.SetActive(true);

            bool canAffordBodykit = cost <= balance;

            if (tooExpensiveOverlay != null)
                tooExpensiveOverlay.SetActive(!canAffordBodykit);
            if (buyButton != null)
            {
                CanvasGroup buyCg = buyButton.GetComponent<CanvasGroup>();
                if (buyCg != null)
                    buyCg.interactable = canAffordBodykit;
            }

            if (selectorPanel != null)
                selectorPanel.SetActive(false);
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
        void HidePurchasingPanel()
        {
            if (purchasingPanel != null)
                purchasingPanel.SetActive(false);
            if (selectorPanel != null)
                selectorPanel.SetActive(true);
        }
        private async void BuyBodykit()
        {
            await TransactionLocker.ProcessTransaction(selectedBodykit, async () =>
            {
                BodykitData bodykitData = StaticData.Bodykits[selectedBodykit];

                if (DataProvider.GameModel.Coins >= bodykitData.BodykitCost)
                {
                    if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                    {
                        // Online purchase
                        try
                        {
                            bool result = await DataProvider.PurchaseBodykitV2(bodykitData.Index);
                            if (result)
                            {
                                PlayerInfoPanelController.Instance.UpdateInfo();
                                HidePurchasingPanel();
                                DataProvider.GameModel.AddBodykit(bodykitData.Index);
                                DataProvider.SaveGame();
                                await DataProvider.CloudSave();
                                ScreensSceneGod.Instance.messageBox.ShowMessage(
                    LocTableCache.ScreensSceneTable.GetString("BodykitPurchased") + " " +
                    LocTableCache.CommonTable.GetString(bodykitData.NameStringKeyBase));
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
                        // Offline purchasing
                        HidePurchasingPanel();
                        DataProvider.GameModel.AddBodykit(bodykitData.Index);
                        ScreensSceneGod.Instance.messageBox.ShowMessage(
            LocTableCache.ScreensSceneTable.GetString("BodykitPurchased") + " " +
            LocTableCache.CommonTable.GetString(bodykitData.NameStringKeyBase));

                        // Subtract from local economy
                        DataProvider.GameModel.Coins -= bodykitData.BodykitCost;
                        PlayerInfoPanelController.Instance.UpdateInfo();

                        // Keep track of transaction for later
                        DataProvider.GameModel.CoinsChange -= bodykitData.BodykitCost;
                        BodykitData bodykit = StaticData.Bodykits[bodykitData.Index];

                        if (DataProvider.GameModel.OutstandingBodykitTransactions == null)
                            DataProvider.GameModel.OutstandingBodykitTransactions = new List<BodykitData>();

                        DataProvider.GameModel.OutstandingBodykitTransactions.Add(bodykit);
                        DataProvider.SaveGame();
                    }
                }
                else
                {
                    // Insufficient funds
#if UNITY_STANDALONE_WIN
ScreensSceneGod.Instance.messageBox.ShowMessage(
LocTableCache.ScreensSceneTable.GetString("InsufficientCoins"), null, null);
#else
                    ScreensSceneGod.Instance.messageBox.ShowMessage(
        LocTableCache.ScreensSceneTable.GetString("InsufficientCoins"),
        GotoBlackMarket,
        LocTableCache.ScreensSceneTable.GetString("GetCoins"));
#endif
                }
            });
        }

        public void GotoBlackMarket()
        {
            GetComponentInParent<ShopPanelScreenController>().GotoBlackMarket();
        }
    }
}

