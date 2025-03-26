using System.Collections.Generic;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Serialization;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.LevelLoot;
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
    public static class DataProvider
    {
        private static readonly ISerializer _serializer = new Serializer(new ModelFilePathProvider());       // functions for local read/write on disk and JSON serialization/deserialization

        public static SettingsManager SettingsManager { get; private set; }
        public static ILockedInformation LockedInfo { get; private set; }

        private static GameModel _gameModel;

        public static IGameModel GameModel => _gameModel;
        public static List<VirtualPurchaseDefinition> m_VirtualPurchaseDefinitions { get; set; }
        public static VirtualShopConfig virtualShopConfig { get; set; }
        public static EcoConfig ecoConfig { get; set; }
        /*     public PvPConfig pvpConfig { get; set; }*/
        public static bool pvpServerAvailable { get; set; }
        static DataProvider()
        {
            if (_serializer.DoesSavedGameExist())
            {
                _gameModel = _serializer.LoadGame();
                if (_gameModel.PlayerLoadout.Is_buildsNull())
                {
                    _gameModel.PlayerLoadout.Create_buildsAnd_units();
                    SaveGame();
                }
                if (_gameModel.PremiumEdition)
                {
                    _gameModel.AddBodykit(0);  // Trident Bodykit000
                    SaveGame();
                }
            }
            else
            {
                // First time run
                _gameModel = StaticData.InitialGameModel;
                SaveGame();
            }

            SettingsManager = new SettingsManager();

            LockedInfo = new LockedInformation(GameModel);
        }

        public static void SaveGame()
        {
            try
            {
                if (_gameModel.NumOfLevelsCompleted > 1)
                    for (int i = 1; i < _gameModel.NumOfLevelsCompleted; i++)
                    {
                        ILoot unlockedLoot = StaticData.GetLevelLoot(i);
                        if (unlockedLoot != null)
                            if (unlockedLoot.Items.Count != 0)
                                foreach (ILootItem lootItem in unlockedLoot.Items)
                                    lootItem.UnlockItem(_gameModel);
                    }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            _serializer.SaveGame(_gameModel);
        }

        public static void Reset()
        {
            _serializer.DeleteSavedGame();
            _serializer.DeleteCloudSave();
        }

        public static async Task CloudSave()
        {
            if (!SettingsManager.CloudSaveDisabled)
            {
                await _serializer.CloudSave(_gameModel);
                Debug.Log("Cloud saved.");
            }
            else
            {
                _serializer.SaveGame(_gameModel);
                Debug.Log("Cloud save disabled. Saving locally instead");
            }
        }

        public static async Task CloudLoad()
        {
            SaveGameModel saveModel = await _serializer.CloudLoad(_gameModel);
            if (saveModel == null || saveModel._lifetimeDestructionScore < _gameModel.LifetimeDestructionScore)
            {
                //override cloud save with local save
                Debug.Log("CloudSaveModel is null.");
                List<Task> syncCurrencyToCloud = new List<Task>
                {
                    SyncCoinsToCloud(),
                    SyncCreditsToCloud()
                };

                _gameModel.CoinsChange = 0;
                _gameModel.CreditsChange = 0;
                _gameModel._outstandingBodykitTransactions = new List<BodykitData>();
                _gameModel._outstandingCaptainTransactions = new List<CaptainData>();
                _gameModel._outstandingHeckleTransactions = new List<HeckleData>();
                _gameModel._outstandingVariantTransactions = new List<VariantData>();

                await Task.WhenAll(syncCurrencyToCloud);
            }
            else if (saveModel._lifetimeDestructionScore > _gameModel.LifetimeDestructionScore)
            {
                saveModel.AssignSaveToGameModel(_gameModel);
                Debug.Log("Cloud save retrieved and applied.");
            }
        }

        public static async Task<bool> SyncCurrencyFromCloud()
        {
            return await _serializer.SyncCurrencyFromCloud();
        }

        public static async Task<bool> SyncInventoryFromCloud()
        {
            return await _serializer.SyncInventoryFromCloud();
        }

        public static async Task<bool> SyncCoinsToCloud()
        {
            return await _serializer.SyncCoinsToCloud();
        }

        public static async Task<bool> SyncCreditsToCloud()
        {
            return await _serializer.SyncCreditsToCloud();
        }

        public static async Task RefreshEconomyConfiguration()
        {
            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            m_VirtualPurchaseDefinitions = EconomyService.Instance.Configuration.GetVirtualPurchases();
        }

        public static async Task ApplyRemoteConfig()
        {
            Debug.Log("ApplyRemoteSettings");
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
            RemoteConfigService.Instance.FetchConfigs(new UserAttributes(), new AppAttributes());
        }

        async static void ApplyRemoteConfig(ConfigResponse configResponse)
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
                    await SyncItemsCostV2();
                    await SyncCurrencyFromCloud();
                    if (_gameModel.IsDoneMigration)
                    {
                        await SyncInventroyV2();
                    }
                    else
                    {
                        await SyncInventoryFromCloud();
                        MigrateInventory();
                        await SyncInventroyV2();
                        _gameModel.IsDoneMigration = true;
                        SaveGame();
                        await CloudSave();
                    }
                    GameModel.HasSyncdShop = true;
                    break;
            }
        }


        public static void MigrateInventory()
        {
            //captain exos
            for (int i = 0; i < _gameModel.PurchasedExos.Count; i++)
                _gameModel.AddExo(_gameModel.PurchasedExos[i]);

            // heckles
            for (int i = 0; i < _gameModel.PurchasedHeckles.Count; i++)
                _gameModel.AddHeckle(_gameModel.PurchasedHeckles[i]);

            // bodykits
            for (int i = 0; i < _gameModel.PurchasedBodykits.Count; i++)
                _gameModel.AddBodykit(_gameModel.PurchasedBodykits[i]);
            // variants
            for (int i = 0; i < _gameModel.PurchasedVariants.Count; i++)
                _gameModel.AddVariant(_gameModel.PurchasedVariants[i]);
        }

        public static async Task SyncInventroyV2()
        {
            // captain exos
            for (int i = 0; i < _gameModel.PurchasedExos.Count; i++)
                _gameModel.AddExo(_gameModel.PurchasedExos[i]);

            // heckles
            for (int i = 0; i < _gameModel.PurchasedHeckles.Count; i++)
                _gameModel.AddHeckle(_gameModel.PurchasedHeckles[i]);

            // bodykits
            try
            {
                for (int i = 0; i < _gameModel.PurchasedBodykits.Count; i++)
                    _gameModel.AddBodykit(_gameModel.PurchasedBodykits[i]);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

            // variants
            for (int i = 0; i < _gameModel.PurchasedVariants.Count; i++)
                _gameModel.AddVariant(_gameModel.PurchasedVariants[i]);

            await Task.CompletedTask;
        }
        private static async Task FetchConfigs()
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

        public static void GetConfigValues()
        {
            // Fetch and deserialize GAME_CONFIG
            var gameConfigsJson = RemoteConfigService.Instance.appConfig.GetJson("GAME_CONFIG");
            Debug.Log($"Fetched GAME_CONFIG: {gameConfigsJson}");
            GameConfig gameConfig = JsonConvert.DeserializeObject<GameConfig>(gameConfigsJson);
            StaticData.GameConfigs = gameConfig.gameconfigs;

            // Fetch and deserialize SHOP_CONFIG
            var shopCategoriesConfigJson = RemoteConfigService.Instance.appConfig.GetJson("SHOP_CONFIG");
            Debug.Log($"Fetched SHOP_CONFIG: {shopCategoriesConfigJson}");
            virtualShopConfig = JsonUtility.FromJson<VirtualShopConfig>(shopCategoriesConfigJson);

            // Fetch and deserialize ECO_CONFIG
            var ecoCategoriesConfigJson = RemoteConfigService.Instance.appConfig.GetJson("ECO_CONFIG");
            Debug.Log($"Fetched ECO_CONFIG: {ecoCategoriesConfigJson}");
            ecoConfig = JsonUtility.FromJson<EcoConfig>(ecoCategoriesConfigJson);

            // Update variant prices from ECO_CONFIG
            for (int i = 0; i < ecoConfig.categories[3].items.Count; i++)
            {
                string credits = ecoConfig.categories[3].items[i].credits;
                int iCredits = 0;
                int.TryParse(credits, out iCredits);
                if (i < StaticData.Variants.Count)
                {
                    StaticData.Variants[i].VariantCredits = iCredits;
                    //Debug.Log($"Updated GameModel Variant {i} Price: {iCredits}");
                }
            }

            // Fetch and deserialize PVP_CONFIG
            var pvpConfigJson = RemoteConfigService.Instance.appConfig.GetJson("PVP_CONFIG");
            Debug.Log($"Fetched PVP_CONFIG: {pvpConfigJson}");
            PvPConfig pvpConfig = JsonUtility.FromJson<PvPConfig>(pvpConfigJson);
            List<Arena> rcArenas = new List<Arena>();
            for (int i = 0; i < pvpConfig.arenas.Count; i++)
            {
                rcArenas.Add(pvpConfig.arenas[i]);
            }
            if (rcArenas != null && rcArenas.Count > 0)
            {
                StaticData.Arenas = rcArenas;
            }

            var pvpQueueName = "bcqueuname"; //RemoteConfigService.Instance.appConfig.GetString("PvP_QUEUE");
            if (_gameModel.QueueName != pvpQueueName)
                _gameModel.QueueName = pvpQueueName;

            var sysReqsJson = RemoteConfigService.Instance.appConfig.GetJson("PVP_REQUIREMENTS");
            Debug.Log($"Fetched PVP_REQUIREMENTS: {sysReqsJson}");
            PvPSysReqs sysReqs = JsonConvert.DeserializeObject<PvPSysReqs>(sysReqsJson);
            StaticData.MinCPUCores = sysReqs.PvPSystemReqs.MinCPUCores;
            StaticData.MinCPUFrequency = sysReqs.PvPSystemReqs.MinCPUFreq;
            StaticData.MaxLatency = sysReqs.PvPSystemReqs.MaxLatency;

            // Save the updated game model
            SaveGame();
        }

        public static string GetPVPVersion()
        {
            var version = RemoteConfigService.Instance.appConfig.GetString("CURRENT_VERSION");
#if UNITY_EDITOR
            version = "EDITOR";
#endif
            return version;
        }

        public static bool RefreshPVPServerStatus()
        {
            pvpServerAvailable = RemoteConfigService.Instance.appConfig.GetBool("PVP_SERVER_AVAILABLE");
            return pvpServerAvailable;
        }

        public static async Task SyncItemsCost()
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
                                    if (cost.id == "COIN")
                                        StaticData.Captains[index].CaptainCost = cost.amount;
                            }
                            if (reward.id.Contains("HECKLE"))
                            {
                                int index = StaticPrefabKeys.HeckleItems[reward.id];
                                foreach (ItemAndAmountSpec cost in costs)
                                    if (cost.id == "COIN")
                                        StaticData.Heckles[index].HeckleCost = cost.amount;
                            }
                            if (reward.id.Contains("BODYKIT"))
                            {
                                int index = StaticPrefabKeys.BodykitItems[reward.id];
                                foreach (ItemAndAmountSpec cost in costs)
                                    if (cost.id == "COIN")
                                        StaticData.Bodykits[index].BodykitCost = cost.amount;
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

        public static async Task SyncItemsCostV2()
        {
            await Task.Yield();
            // captainexos cost sync
            for (int i = 0; i < ecoConfig.categories[0].items.Count; i++)
            {
                string coins = ecoConfig.categories[0].items[i].coins;
                int iCoins = 0;
                int.TryParse(coins, out iCoins);
                if (i < StaticData.Captains.Count)
                    StaticData.Captains[i].CaptainCost = iCoins;
            }
            // heckles cost sync
            for (int i = 0; i < ecoConfig.categories[1].items.Count; i++)
            {
                string coins = ecoConfig.categories[1].items[i].coins;
                int iCoins = 0;
                int.TryParse(coins, out iCoins);
                if (i < StaticData.Heckles.Count)
                    StaticData.Heckles[i].HeckleCost = iCoins;
            }
            // bodykits cost sync
            for (int i = 0; i < ecoConfig.categories[2].items.Count; i++)
            {
                string coins = ecoConfig.categories[2].items[i].coins;
                int iCoins = 0;
                int.TryParse(coins, out iCoins);
                if (i < StaticData.Bodykits.Count)
                    StaticData.Bodykits[i].BodykitCost = iCoins;
            }
            // variant cost async
            for (int i = 0; i < ecoConfig.categories[3].items.Count; i++)
            {
                string credits = ecoConfig.categories[3].items[i].credits;
                int iCredits = 0;
                int.TryParse(credits, out iCredits);
                if (i < StaticData.Variants.Count)
                    StaticData.Variants[i].VariantCredits = iCredits;
            }
            SaveGame();
        }

        public static async Task SyncHecklesCost()
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
                                    if (spec.id == "COIN")
                                        StaticData.Heckles[i + 3].HeckleCost = spec.amount;
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

        public static async Task<bool> PurchaseCaptain(int index)
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

        public static async Task<bool> PurchaseCaptainV2(int index)
        {
            Assert.IsTrue(index > 0); // 0 is default item. can not buy them.
            await Task.Yield();
            int iCoins = StaticData.Captains[index].CaptainCost;
            _gameModel.Coins -= iCoins;
            SaveGame();
            await SyncCoinsToCloud();
            return true;
        }

        public static async Task<bool> PurchaseHeckle(int index)
        {
            //    Assert.IsTrue(index > 2); // 0,1,2 are default items. can not buy them.
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

        public static async Task<bool> PurchaseHeckleV2(int index)
        {
            Assert.IsTrue(index >= 0);
            await Task.Yield();
            int iCoins = StaticData.Heckles[index].HeckleCost;
            _gameModel.Coins -= iCoins;
            SaveGame();
            await SyncCoinsToCloud();
            return true;
        }
        public static async Task<bool> PurchaseBodykit(int index)
        {
            Assert.IsTrue(index > 0); // 0 is trident for premium
            try
            {
                string purchaseId = virtualShopConfig.categories[3].items[index - 1].id;  // category 3 is bodykit
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

        public static async Task<bool> PurchaseBodykitV2(int index)
        {
            Assert.IsTrue(index > 0); // 0 is trident for premium
            await Task.Yield();
            int iCoins = StaticData.Bodykits[index].BodykitCost;
            _gameModel.Coins -= iCoins;
            SaveGame();
            await SyncCoinsToCloud();
            return true;
        }

        public static async Task<bool> PurchaseVariant(int index)
        {
            Assert.IsTrue(index >= 0);
            await Task.Yield();
            int iCredits = StaticData.Variants[index].VariantCredits;
            _gameModel.Credits -= iCredits;
            SaveGame();
            await SyncCreditsToCloud();
            return true;
        }

        static List<ItemAndAmountSpec> ParseEconomyItems(List<PurchaseItemQuantity> itemQuantities)
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
        public static async Task ProcessOfflineTransactions()
        {
            await SyncCurrencyFromCloud();
            // these two lines are duplicated from GetConfigValues():
            var shopCategoriesConfigJson = RemoteConfigService.Instance.appConfig.GetJson("SHOP_CONFIG");
            virtualShopConfig = JsonUtility.FromJson<VirtualShopConfig>(shopCategoriesConfigJson);

            Debug.Log("Processing offline transactions.");
            int runningCoinTotal = (int)GameModel.Coins;
            int runningCreditTotal = (int)GameModel.Credits;  // SyncCurrencyFromCloud means these start from what the cloud provides.
            int coinLocalTotal = (int)GameModel.Coins + GameModel.CoinsChange;
            int creditLocalTotal = (int)GameModel.Credits + GameModel.CreditsChange;

            // Surplus: If a LocalTotal is positive, local and cloud economy can coexist. We can just process everything without issue.
            // Deficit: If a LocalTotal is negative then some local spending is incompatible with the cloud economy, and needs to be discarded.

            // Coin transactions
            if (coinLocalTotal >= 0)
            {
                Debug.Log("Offline coin transactions do not conflict.");
                // Surplus
                // All coin transactions can resolve.
                // These methods handle buying everything on the Outstanding Transactions lists:
                if (GameModel.OutstandingBodykitTransactions != null && GameModel.OutstandingBodykitTransactions.Count > 0)
                {
                    await ProcessOfflineBodykits();
                }
                if (GameModel.OutstandingCaptainTransactions != null && GameModel.OutstandingCaptainTransactions.Count > 0)
                {
                    await ProcessOfflineCaptains();
                }
                if (GameModel.OutstandingHeckleTransactions != null && GameModel.OutstandingHeckleTransactions.Count > 0)
                {
                    await ProcessOfflineHeckles();
                }

                // handle any winnings from offline pve games:
                GameModel.Coins += GameModel.CoinsChange;
                GameModel.CoinsChange = 0;
                await SyncCoinsToCloud();
            }
            else
            {
                Debug.Log("Offline transaction conflict!");
                // Deficit
                // Needs to be handled

                // Bodykits first
                if (GameModel.OutstandingBodykitTransactions != null && GameModel.OutstandingBodykitTransactions.Count > 0)
                {
                    runningCoinTotal = await ProcessOfflineBodykitsConflicts(runningCoinTotal);
                }

                // Captains second
                if (GameModel.OutstandingCaptainTransactions != null && GameModel.OutstandingCaptainTransactions.Count > 0)
                {
                    runningCoinTotal = await ProcessOfflineCaptainsConflicts(runningCoinTotal);
                }

                // Heckles third
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

                // Surplus
                // All credit transactions can resolve.
                // These methods handle buying everything on the Outstanding Transactions lists:
                if (GameModel.OutstandingVariantTransactions != null && GameModel.OutstandingVariantTransactions.Count > 0)
                {
                    await ProcessOfflineVariants();
                }

                // handle any winnings from offline pve games:
                GameModel.Credits += GameModel.CreditsChange;
                GameModel.CreditsChange = 0;
                await SyncCreditsToCloud();
            }
            else
            {
                Debug.Log("Offline transaction conflict!");
                // Deficit
                // Needs to be handled

                // Variants first
                if (GameModel.OutstandingVariantTransactions != null && GameModel.OutstandingVariantTransactions.Count > 0)
                {
                    runningCreditTotal = await ProcessOfflineVariantsConflicts(runningCreditTotal);
                }

                // handle any remainder:
                GameModel.Credits += runningCreditTotal;
                GameModel.CreditsChange = 0;
            }

            SaveGame();
            await CloudSave();
        }

        // Officially buys all the Bodykits that were bought offline:
        private static async Task ProcessOfflineBodykits()
        {
            List<BodykitData> RetryBodykits = new List<BodykitData>();

            // Captains
            foreach (BodykitData txn in GameModel.OutstandingBodykitTransactions)
            {
                Debug.Log("Purchasing Bodykit " + txn.Index);
                bool result = await PurchaseBodykitV2(txn.Index);
                if (result)
                {
                    GameModel.AddBodykit(txn.Index);
                    GameModel.CoinsChange += txn.BodykitCost;
                }
                else
                {
                    Debug.LogWarning("FAILED: Purchasing Bodykit " + txn.Index + ", will retry next time the game is run.");
                    RetryBodykits.Add(txn);
                }
            }
            //   await SyncCurrencyFromCloud();
            // If any failed, they'll be preserved for next connection:
            if (RetryBodykits != null) { GameModel.OutstandingBodykitTransactions = RetryBodykits; }
            else { GameModel.OutstandingBodykitTransactions = new List<BodykitData>(); }
        }

        // Officially buys Captains based on what was bought offline, but only as many as the cloud economy allows:
        private static async Task<int> ProcessOfflineBodykitsConflicts(int runningCoinTotal)
        {
            List<int> GoodBodykits = new List<int>();

            foreach (BodykitData txn in GameModel.OutstandingBodykitTransactions)
            {
                if (runningCoinTotal - txn.BodykitCost >= 0)
                {
                    runningCoinTotal -= txn.BodykitCost;
                    GoodBodykits.Add(txn.Index);
                }
                else
                {
                    Debug.Log("Reverting purchase of Bodykit " + txn.Index);
                    GameModel.RemoveBodykit(txn.Index);
                }
            }
            GameModel.OutstandingBodykitTransactions = new List<BodykitData>();

            if (GoodBodykits != null && GoodBodykits.Count > 0)
            {
                foreach (int bdk in GoodBodykits)
                {
                    Debug.Log("Purchasing Bodykit " + bdk);
                    bool result = await PurchaseBodykitV2(bdk);
                    if (result)
                    {
                        //    await SyncCurrencyFromCloud();
                        GameModel.AddBodykit(bdk);
                    }
                }
            }
            return runningCoinTotal;
        }

        // Officially buys all the Captains that were bought offline:
        private static async Task ProcessOfflineCaptains()
        {
            List<CaptainData> RetryCaptains = new List<CaptainData>();

            // Captains
            foreach (CaptainData txn in GameModel.OutstandingCaptainTransactions)
            {
                Debug.Log("Purchasing Captain " + txn.Index);
                bool result = await PurchaseCaptainV2(txn.Index);
                if (result)
                {
                    GameModel.AddExo(txn.Index);
                    GameModel.CoinsChange += txn.CaptainCost;
                }
                else
                {
                    Debug.LogWarning("FAILED: Purchasing Captain " + txn.Index + ", will retry next time the game is run.");
                    RetryCaptains.Add(txn);
                }
            }
            //    await SyncCurrencyFromCloud();
            // If any failed, they'll be preserved for next connection:
            if (RetryCaptains != null) { GameModel.OutstandingCaptainTransactions = RetryCaptains; }
            else { GameModel.OutstandingCaptainTransactions = new List<CaptainData>(); }
        }

        // Officially buys Captains based on what was bought offline, but only as many as the cloud economy allows:
        private static async Task<int> ProcessOfflineCaptainsConflicts(int runningCoinTotal)
        {
            List<int> GoodCaptains = new List<int>();

            foreach (CaptainData txn in GameModel.OutstandingCaptainTransactions)
            {
                if (runningCoinTotal - txn.CaptainCost >= 0)
                {
                    runningCoinTotal -= txn.CaptainCost;
                    GoodCaptains.Add(txn.Index);
                }
                else
                {
                    Debug.Log("Reverting purchase of Captain " + txn.Index);
                    GameModel.RemoveExo(txn.Index);
                }
            }
            GameModel.OutstandingCaptainTransactions = new List<CaptainData>();

            if (GoodCaptains != null && GoodCaptains.Count > 0)
            {
                foreach (int cpt in GoodCaptains)
                {
                    Debug.Log("Purchasing Captain " + cpt);
                    bool result = await PurchaseCaptainV2(cpt);
                    if (result)
                    {
                        //    await SyncCurrencyFromCloud();
                        GameModel.AddExo(cpt);
                    }
                }
            }

            return runningCoinTotal;
        }

        // Officially buys all the Heckles that were bought offline:
        private static async Task ProcessOfflineHeckles()
        {
            List<HeckleData> RetryHeckles = new List<HeckleData>();

            foreach (HeckleData txn in GameModel.OutstandingHeckleTransactions)
            {
                Debug.Log("Purchasing Heckle " + txn.Index);
                bool result = await PurchaseHeckleV2(txn.Index);
                if (result)
                {

                    GameModel.AddHeckle(txn.Index);
                    GameModel.CoinsChange += txn.HeckleCost;
                }
                else
                {
                    Debug.LogWarning("FAILED: Purchasing Heckle " + txn.Index + ", will retry next time the game is run.");
                    RetryHeckles.Add(txn);
                }
            }
            //    await SyncCurrencyFromCloud();
            // If any failed, they'll be preserved for next connection:
            if (RetryHeckles != null) { GameModel.OutstandingHeckleTransactions = RetryHeckles; }
            else { GameModel.OutstandingHeckleTransactions = new List<HeckleData>(); }
        }

        // Officially buys Heckles based on what was bought offline, but only as many as the cloud economy allows:
        private static async Task<int> ProcessOfflineHecklesConflicts(int runningCoinTotal)
        {
            List<int> GoodHeckles = new List<int>();

            foreach (HeckleData heckle in GameModel.OutstandingHeckleTransactions)
            {
                if (runningCoinTotal - heckle.HeckleCost >= 0)
                {
                    runningCoinTotal -= heckle.HeckleCost;
                    GoodHeckles.Add(heckle.Index);
                }
                else
                {
                    Debug.Log("Reverting purchase of Heckle " + heckle.Index);
                    GameModel.RemoveHeckle(heckle.Index);
                }
            }
            GameModel.OutstandingHeckleTransactions = new List<HeckleData>();

            if (GoodHeckles != null && GoodHeckles.Count > 0)
            {
                foreach (int heckle in GoodHeckles)
                {
                    Debug.Log("Purchasing Heckle " + heckle);
                    bool result = await PurchaseHeckleV2(heckle);
                    if (result)
                    {
                        //    await SyncCurrencyFromCloud();
                        GameModel.AddHeckle(heckle);
                    }
                }
            }
            return runningCoinTotal;
        }

        // Officially buys all the Variants that were bought offline:
        private static async Task ProcessOfflineVariants()
        {
            List<VariantData> RetryVariants = new List<VariantData>();

            foreach (VariantData txn in GameModel.OutstandingVariantTransactions)
            {
                Debug.Log("Purchasing Variant " + txn.Index);
                bool result = await PurchaseVariant(txn.Index);
                if (result)
                {
                    GameModel.AddVariant(txn.Index);
                    GameModel.CreditsChange += txn.VariantCredits;
                }
                else
                {
                    Debug.LogWarning("FAILED: Purchasing Variant " + txn.Index + ", will retry next time the game is run.");
                    RetryVariants.Add(txn);
                }
            }
            //    await SyncCurrencyFromCloud();
            // If any failed, they'll be preserved for next connection:
            if (RetryVariants != null) { GameModel.OutstandingVariantTransactions = RetryVariants; }
            else { GameModel.OutstandingVariantTransactions = new List<VariantData>(); }
        }

        // Officially buys Variants based on what was bought offline, but only as many as the cloud economy allows:
        private static async Task<int> ProcessOfflineVariantsConflicts(int runningCreditTotal)
        {
            List<int> GoodVariants = new List<int>();

            foreach (VariantData txn in GameModel.OutstandingVariantTransactions)
            {
                if (runningCreditTotal - txn.VariantCredits >= 0)
                {
                    runningCreditTotal -= txn.VariantCredits;
                    GoodVariants.Add(txn.Index);
                }
                else
                {
                    Debug.Log("Reverting purchase of Variant " + txn.Index);
                    GameModel.RemoveVariant(txn.Index);
                }
            }
            GameModel.OutstandingVariantTransactions = new List<VariantData>();

            if (GoodVariants != null && GoodVariants.Count > 0)
            {
                foreach (int vnt in GoodVariants)
                {
                    Debug.Log("Purchasing Variant " + vnt);
                    bool result = await PurchaseVariant(vnt);
                    if (result)
                    {
                        //    await SyncCurrencyFromCloud();
                        GameModel.AddVariant(vnt);
                    }
                }
            }
            return runningCreditTotal;
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
    public struct PriceConfig
    {
        public string coins;
        public string credits;
        public override string ToString()
        {
            var returnString = new StringBuilder($"\"{coins} --- {credits}\"");
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
    public struct ItemCategory
    {
        public string id;
        public bool enabledFlag;
        public List<PriceConfig> items;
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
    [Serializable]
    public struct EcoConfig
    {
        public List<ItemCategory> categories;
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

    [Serializable]
    public struct PvPSysReqs
    {
        public PvPSystemRequirements PvPSystemReqs { get; set; }
    }

    [Serializable]
    public struct PvPSystemRequirements
    {
        public int MinCPUCores { get; set; }
        public int MinCPUFreq { get; set; }
        public int MaxLatency { get; set; }
    }

    struct UserAttributes { }

    struct AppAttributes { }
}
