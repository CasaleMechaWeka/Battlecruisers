using System.Collections.Generic;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Serialization;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using UnityEngine.Assertions;
using System.Threading.Tasks;
using Unity.Services.Economy.Model;
using Unity.Services.RemoteConfig;
using Unity.Services.Economy;
using System;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using BattleCruisers.Scenes;
using Unity.Services.Authentication;
using BattleCruisers.Utils.UGS.Samples;
using BattleCruisers.UI.ScreensScene.ShopScreen;

namespace BattleCruisers.Data
{
    public class DataProvider : IDataProvider
    {
        private readonly ISerializer _serializer;       // functions for local read/write on disk and JSON serialization/deserialization
        private readonly ISaveClient _cloudSaveService; // cloud save serialized JSON

        public IStaticData StaticData { get; }
        public IList<ILevel> Levels => StaticData.Levels;
        public IDictionary<Map, IPvPLevel> PvPLevels => StaticData.PvPLevels;
        public ISettingsManager SettingsManager { get; }
        public ILockedInformation LockedInfo { get; }

        private readonly GameModel _gameModel;

        public IGameModel GameModel => _gameModel;
        public List<VirtualPurchaseDefinition> m_VirtualPurchaseDefinitions { get; set; }
        public VirtualShopConfig virtualShopConfig { get; set; }
        /*     public PvPConfig pvpConfig { get; set; }*/
        public bool pvpServerAvailable { get; set; }
        public DataProvider(IStaticData staticData, ISerializer serializer)
        {
            Helper.AssertIsNotNull(staticData, serializer);

            StaticData = staticData;
            _serializer = serializer;

            if (_serializer.DoesSavedGameExist())
            {
                _gameModel = _serializer.LoadGame();
                if (_gameModel.PlayerLoadout.Is_buildsNull())
                {
                    _gameModel.PlayerLoadout.Create_buildsAnd_units();
                    SaveGame();
                }
            }
            else
            {
                // First time run
                _gameModel = StaticData.InitialGameModel;
                SaveGame();
            }

            SettingsManager = new SettingsManager(this);

            LockedInfo = new LockedInformation(GameModel, StaticData);
        }

        public ILevel GetLevel(int levelNum)
        {
            Assert.IsTrue(levelNum > 0 && levelNum <= Levels.Count);
            return Levels[levelNum - 1];
        }

        public IPvPLevel GetPvPLevel(Map map)
        {
            // Assert.IsTrue(levelNum > 0 && levelNum <= PvPLevels.Count);
            return PvPLevels[map];
        }

        public void SaveGame()
        {
            _serializer.SaveGame(_gameModel);
        }

        public void Reset()
        {
            _serializer.DeleteSavedGame();
            _serializer.DeleteCloudSave();
        }

        public async Task CloudSave()
        {
            await _serializer.CloudSave(_gameModel);
            Debug.Log("Cloud saved.");
        }

        public async Task CloudLoad()
        {
            SaveGameModel saveModel = await _serializer.CloudLoad(_gameModel);
            if (saveModel == null)
            {
                Debug.Log("CloudSaveModel is null.");
            }
            else
            {
                saveModel.AssignSaveToGameModel(_gameModel);
                Debug.Log("Cloud save retrieved and applied.");
            }
        }

        public async Task<bool> SyncCurrencyFromCloud()
        {
            return await _serializer.SyncCurrencyFromCloud(this);
        }

        public async Task<bool> SyncInventoryFromCloud()
        {
            return await _serializer.SyncInventoryFromCloud(this);
        }

        public async Task<bool> SyncCoinsToCloud()
        {
            return await _serializer.SyncCoinsToCloud(this);
        }

        public async Task<bool> SyncCreditsToCloud()
        {
            return await _serializer.SyncCreditsToCloud(this);
        }

        private async Task RefreshEconomyConfiguration()
        {
            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            m_VirtualPurchaseDefinitions = EconomyService.Instance.Configuration.GetVirtualPurchases();
        }

        public async Task LoadBCData()
        {
            await RefreshEconomyConfiguration();
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            RemoteConfigService.Instance.FetchConfigs(new UserAttributes(), new AppAttributes());
        }
        async void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            switch (configResponse.requestOrigin)
            {
                case ConfigOrigin.Default:
                    Debug.Log("===> config.Default");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log("===> config.Cached");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log("===> config.Remote");
                    GetConfigValues();
                    await SyncItemsCost();
            //        await SyncHecklesCost();
                    await SyncCurrencyFromCloud();
                    await SyncInventoryFromCloud();
                    GameModel.HasSyncdShop = true;
                    ScreensSceneGod.Instance.m_cancellationToken.Cancel();
                    break;
            }
        }
        private async Task FetchConfigs()
        {
            try
            {
                await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
                GetConfigValues();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void GetConfigValues()
        {
            var gameConfigsJson = RemoteConfigService.Instance.appConfig.GetJson("GAME_CONFIG");
            GameConfig gameConfig = JsonConvert.DeserializeObject<GameConfig>(gameConfigsJson);
            _gameModel.GameConfigs = gameConfig.gameconfigs;

            var shopCategoriesConfigJson = RemoteConfigService.Instance.appConfig.GetJson("SHOP_CONFIG");
            virtualShopConfig = JsonUtility.FromJson<VirtualShopConfig>(shopCategoriesConfigJson);

            var pvpConfigJson = RemoteConfigService.Instance.appConfig.GetJson("PVP_CONFIG");
            PvPConfig pvpConfig = JsonUtility.FromJson<PvPConfig>(pvpConfigJson);
            List<Arena> rcArenas = new List<Arena>();
            for (int i = 0; i < pvpConfig.arenas.Count; i++)
            {
                rcArenas.Add(pvpConfig.arenas[i]);
            }
            if (rcArenas != null && rcArenas.Count > 0)
            {
                _gameModel.Arenas = rcArenas;
            }

            var pvpQueueName = RemoteConfigService.Instance.appConfig.GetString("PvP_QUEUE");
            if (_gameModel.QueueName != pvpQueueName)
                _gameModel.QueueName = pvpQueueName;
            SaveGame();
        }

        public async Task<string> GetPVPVersion()
        {
            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            var version = RemoteConfigService.Instance.appConfig.GetString("CURRENT_VERSION");

            #if UNITY_EDITOR
            version = "EDITOR";
            #endif

            return version;
        }

        public async Task<bool> RefreshPVPServerStatus()
        {
            pvpServerAvailable = RemoteConfigService.Instance.appConfig.GetBool("PVP_SERVER_AVAILABLE");
            return pvpServerAvailable;
        }

        public async Task SyncItemsCost()
        {
            if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    foreach (VirtualPurchaseDefinition purchaseDef in m_VirtualPurchaseDefinitions)
                    {
                        var rewards = ParseEconomyItems(purchaseDef.Rewards);
                        var costs = ParseEconomyItems(purchaseDef.Costs);
                        foreach (ItemAndAmountSpec reward in rewards)
                        {
                            if (reward.id.Contains("CAPTAIN"))
                            {
                                int index = StaticPrefabKeys.CaptainItems[reward.id];
                                foreach (ItemAndAmountSpec cost in costs)
                                {
                                    if (cost.id == "COIN")
                                        _gameModel.Captains[index].captainCost = cost.amount;
                                }
                            }
                            if (reward.id.Contains("HECKLE"))
                            {
                                int index = StaticPrefabKeys.HeckleItems[reward.id];
                                foreach (ItemAndAmountSpec cost in costs)
                                {
                                    if (cost.id == "COIN")
                                        _gameModel.Heckles[index].heckleCost = cost.amount;
                                }
                            }
                            if(reward.id.Contains("BODYKIT"))
                            {
                                int index = StaticPrefabKeys.BodykitItems[reward.id];
                                foreach(ItemAndAmountSpec cost in costs)
                                {
                                    if(cost.id == "COIN")
                                        _gameModel.Bodykits[index].bodykitCost = cost.amount;
                                }
                            }
                        }
                    }
                    SaveGame();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }

        public async Task SyncHecklesCost()
        {
            if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    for (int i = 0; i < virtualShopConfig.categories[1].items.Count; i++)
                    {
                        string targetPurchaseID = virtualShopConfig.categories[1].items[i].id;
                        foreach (VirtualPurchaseDefinition purchaseDef in m_VirtualPurchaseDefinitions)
                        {
                            if (targetPurchaseID == purchaseDef.Id)
                            {
                                var costs = ParseEconomyItems(purchaseDef.Costs);
                                foreach (ItemAndAmountSpec spec in costs)
                                {
                                    if (spec.id == "COIN")
                                        _gameModel.Heckles[i + 3].heckleCost = spec.amount;
                                }
                            }
                        }
                    }
                    SaveGame();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }


        /*        public async Task<bool> PurchaseIAP(string purchaseId)
                {
                    try 
                    { 
                        var result =  await EconomyManager.
                    }
                }*/
        public async Task<bool> PurchaseCaptain(int index)
        {
            Assert.IsTrue(index > 0); // 0 is default item. can not buy them.
            try
            {
                string purchaseId = virtualShopConfig.categories[0].items[index - 1].id;   // category 0 is captains
                var result = await EconomyManager.MakeVirtualPurchaseAsync(purchaseId);
                if (result == null)
                    return false;
                await EconomyManager.RefreshCurrencyBalances();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PurchaseHeckle(int index)
        {
            Assert.IsTrue(index > 2); // 0,1,2 are default items. can not buy them.
            try
            {
                string purchaseId = virtualShopConfig.categories[1].items[index - 3].id;  // category 1 is heckles
                var result = await EconomyManager.MakeVirtualPurchaseAsync(purchaseId);
                if (result == null)
                    return false;
                await EconomyManager.RefreshCurrencyBalances();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> PurchaseBodykit(int index)
        {
            Assert.IsTrue(index > 0); // 0 is trident for premium
            try
            {
                string purchaseId = virtualShopConfig.categories[3].items[index-1].id;  // category 3 is bodykit
                var result = await EconomyManager.MakeVirtualPurchaseAsync(purchaseId);
                if (result == null)
                    return false;
                await EconomyManager.RefreshCurrencyBalances();
                return true;
            }
            catch
            {
                return false;
            }
        }

        List<ItemAndAmountSpec> ParseEconomyItems(List<PurchaseItemQuantity> itemQuantities)
        {
            var itemsAndAmountsSpec = new List<ItemAndAmountSpec>();

            foreach (var itemQuantity in itemQuantities)
            {
                var id = itemQuantity.Item.GetReferencedConfigurationItem().Id;
                itemsAndAmountsSpec.Add(new ItemAndAmountSpec(id, itemQuantity.Amount));
            }

            return itemsAndAmountsSpec;
        }

        // Any shop items bought while offline need to be accounted for and either merged with cloud economy or discarded
        public async Task ProcessOfflineTransactions()
        {
            await SyncCurrencyFromCloud();
            // these two lines are duplicated from GetConfigValues():
            var shopCategoriesConfigJson = RemoteConfigService.Instance.appConfig.GetJson("SHOP_CONFIG");
            virtualShopConfig = JsonUtility.FromJson<VirtualShopConfig>(shopCategoriesConfigJson);

            Debug.Log("Processing offline transactions.");
            int runningCoinTotal = (int)GameModel.Coins;  // SyncCurrencyFromCloud means these start from what the cloud provides.
            int coinLocalTotal = (int)GameModel.Coins + GameModel.CoinsChange;
            int creditLocalTotal = (int)GameModel.Credits + GameModel.CreditsChange;
            //int runningCreditTotal
            //int creditLocalTotal

            // Surplus: If a LocalTotal is positive, local and cloud economy can coexist. We can just process everything without issue.
            // Deficit: If a LocalTotal is negative then some local spending is incompatible with the cloud economy, and needs to be discarded.

            // Coin transactions
            if (coinLocalTotal >= 0)
            {
                Debug.Log("Offline coin transactions do not conflict.");
                // Surplus
                // All coin transactions can resolve.
                // These methods handle buying everything on the Outstanding Transactions lists:
                if (GameModel.OutstandingCaptainTransactions != null && GameModel.OutstandingCaptainTransactions.Count > 0)
                {
                    await ProcessOfflineCaptains();
                    await ProcessOfflineHeckles();
                }

                // handle any winnings from offline pve games:
                GameModel.Coins += GameModel.CoinsChange;
                GameModel.CoinsChange = 0;
                await SyncCoinsToCloud();
            }
            else
            {
                // Deficit
                // Needs to be handled
                // Captains first
                Debug.Log("Offline transaction conflict!");
                if (GameModel.OutstandingCaptainTransactions != null && GameModel.OutstandingCaptainTransactions.Count > 0)
                {
                    runningCoinTotal = await ProcessOfflineCaptainsConflicts(runningCoinTotal);
                }

                // Heckles second
                if (GameModel.OutstandingHeckleTransactions != null && GameModel.OutstandingHeckleTransactions.Count > 0)
                {
                    runningCoinTotal = await ProcessOfflineHecklesConflicts(runningCoinTotal);

                }

                // handle any remainder:
                GameModel.Coins += runningCoinTotal;
                GameModel.CoinsChange = 0;
            }

            // Credit transactions
            if (creditLocalTotal >= 0)
            {
                Debug.Log("Offline credits transactions do not conflict.");

                // handle any winnings from offline pve games:
                GameModel.Credits += GameModel.CreditsChange;
                GameModel.CreditsChange = 0;
                await SyncCreditsToCloud();
            }

            SaveGame();
            await CloudSave();
        }

        // Officially buys all the Captains that were bought offline:
        private async Task ProcessOfflineCaptains()
        {
            List<CaptainData> RetryCaptains = new List<CaptainData>();

            // Captains
            foreach (CaptainData txn in GameModel.OutstandingCaptainTransactions)
            {
                Debug.Log("Purchasing Captain " + txn.index);
                bool result = await PurchaseCaptain(txn.index);
                if (result)
                {
                    GameModel.Captains[txn.index].isOwned = true;
                    GameModel.CoinsChange += txn.captainCost;
                }
                else
                {
                    Debug.LogWarning("FAILED: Purchasing Captain " + txn.index + ", will retry next time the game is run.");
                    RetryCaptains.Add(txn);
                }
            }
            await SyncCurrencyFromCloud();
            // If any failed, they'll be preserved for next connection:
            if (RetryCaptains != null) { GameModel.OutstandingCaptainTransactions = RetryCaptains; }
            else { GameModel.OutstandingCaptainTransactions = new List<CaptainData>(); }
        }

        // Officially buys Captains based on what was bought offline, but only as many as the cloud economy allows:
        private async Task<int> ProcessOfflineCaptainsConflicts(int runningCoinTotal)
        {
            List<int> GoodCaptains = new List<int>();

            foreach (CaptainData txn in GameModel.OutstandingCaptainTransactions)
            {
                if (runningCoinTotal - txn.captainCost >= 0)
                {
                    runningCoinTotal -= txn.captainCost;
                    GoodCaptains.Add(txn.index);
                }
                else
                {
                    Debug.Log("Reverting purchase of Captain " + txn.index);
                    GameModel.Captains[txn.index].isOwned = false;
                }
            }
            GameModel.OutstandingCaptainTransactions = new List<CaptainData>();

            if (GoodCaptains != null && GoodCaptains.Count > 0)
            {
                foreach (int cpt in GoodCaptains)
                {
                    Debug.Log("Purchasing Captain " + cpt);
                    bool result = await PurchaseCaptain(cpt);
                    if (result)
                    {
                        await SyncCurrencyFromCloud();
                        GameModel.Captains[cpt].isOwned = true;
                    }
                }
            }

            return runningCoinTotal;
        }

        // Officially buys all the Heckles that were bought offline:
        private async Task ProcessOfflineHeckles()
        {
            List<HeckleData> RetryHeckles = new List<HeckleData>();

            foreach (HeckleData txn in GameModel.OutstandingHeckleTransactions)
            {
                Debug.Log("Purchasing Heckle " + txn.index);
                bool result = await PurchaseHeckle(txn.index);
                if (result)
                {

                    GameModel.Heckles[txn.index].isOwned = true;
                    GameModel.CoinsChange += txn.heckleCost;
                }
                else
                {
                    Debug.LogWarning("FAILED: Purchasing Heckle " + txn.index + ", will retry next time the game is run.");
                    RetryHeckles.Add(txn);
                }
            }
            await SyncCurrencyFromCloud();
            // If any failed, they'll be preserved for next connection:
            if (RetryHeckles != null) { GameModel.OutstandingHeckleTransactions = RetryHeckles; }
            else { GameModel.OutstandingHeckleTransactions = new List<HeckleData>(); }
        }

        // Officially buys Heckles based on what was bought offline, but only as many as the cloud economy allows:
        private async Task<int> ProcessOfflineHecklesConflicts(int runningCoinTotal)
        {
            List<int> GoodHeckles = new List<int>();

            foreach (HeckleData txn in GameModel.OutstandingHeckleTransactions)
            {
                if (runningCoinTotal - txn.heckleCost >= 0)
                {
                    runningCoinTotal -= txn.heckleCost;
                    GoodHeckles.Add(txn.index);
                }
                else
                {
                    Debug.Log("Reverting purchase of Heckle " + txn.index);
                    GameModel.Heckles[txn.index].isOwned = false;
                }
            }
            GameModel.OutstandingHeckleTransactions = new List<HeckleData>();

            if (GoodHeckles != null && GoodHeckles.Count > 0)
            {
                foreach (int hkl in GoodHeckles)
                {
                    Debug.Log("Purchasing Heckle " + hkl);
                    bool result = await PurchaseHeckle(hkl);
                    if (result)
                    {
                        await SyncCurrencyFromCloud();
                        GameModel.Heckles[hkl].isOwned = true;
                    }
                }
            }
            return runningCoinTotal;
        }
    }

    [Serializable]
    public struct ItemConfig
    {
        public string id;
        public override string ToString()
        {
            var returnString = new StringBuilder($"\"{id}\"");
            return returnString.ToString();
        }
    }

    [Serializable]
    public struct CategoryConfig
    {
        public string id;
        public bool enabledFlag;
        public List<ItemConfig> items;
        public override string ToString()
        {
            var returnString = new StringBuilder($"category:\"{id}\", enabled:{enabledFlag}");
            if (items?.Count > 0)
            {
                returnString.Append($", items: {string.Join(", ", items.Select(itemConfig => itemConfig.ToString()).ToArray())}");
            }

            return returnString.ToString();
        }
    }

    [Serializable]
    public struct VirtualShopConfig
    {
        public List<CategoryConfig> categories;

        public override string ToString()
        {
            return $"categories: {string.Join(", ", categories.Select(category => category.ToString()).ToArray())}";
        }
    }

    public struct ItemAndAmountSpec
    {
        public string id;
        public int amount;

        public ItemAndAmountSpec(string id, int amount)
        {
            this.id = id;
            this.amount = amount;
        }
        public override string ToString()
        {
            return $"{id}:{amount}";
        }
    }

    [Serializable]
    public struct PvPConfig
    {
        public List<Arena> arenas;
    }

    [Serializable]
    public struct Arena
    {
        public string id;
        public int costcoins;
        public int costcredits;
        public int prizecoins;
        public int prizecredits;
        public int prizenukes;
        public int consolationcredits;
        public int consolationnukes;

        public Arena(
        string id = "Template",
        int costcoins = 0,
        int costcredits = 0,
        int prizecoins = 0,
        int prizecredits = 0,
        int prizenukes = 0,
        int consolationcredits = 0,
        int consolationnukes = 0)
        {
            this.id = id;
            this.costcoins = costcoins;
            this.costcredits = costcredits;
            this.prizecoins = prizecoins;
            this.prizecredits = prizecredits;
            this.prizenukes = prizenukes;
            this.consolationcredits = consolationcredits;
            this.consolationnukes = consolationnukes;
        }
    }

    [Serializable]
    public struct GameConfig
    {
        public Dictionary<string, int> gameconfigs;
    }

    struct UserAttributes { }

    struct AppAttributes { }
}
