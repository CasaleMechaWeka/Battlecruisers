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
using UnityEngine;
using BattleCruisers.Scenes;
using Unity.Services.Authentication;
using BattleCruisers.Utils.UGS.Samples;

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

        private GameModel _gameModel;
        public IGameModel GameModel => _gameModel;
        public List<VirtualPurchaseDefinition> m_VirtualPurchaseDefinitions { get; set; }
        public VirtualShopConfig virtualShopConfig { get; set; }
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
        }

        public async Task CloudLoad()
        {
            GameModel gModelFromCloud = await _serializer.CloudLoad();
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
                    await SyncCaptainsCost();
                    await SyncHecklesCost();
                    await SyncCurrencyFromCloud();
                    await SyncInventoryFromCloud();
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

        void GetConfigValues()
        {
            var shopCategoriesConfigJson = RemoteConfigService.Instance.appConfig.GetJson("SHOP_CONFIG");
            virtualShopConfig = JsonUtility.FromJson<VirtualShopConfig>(shopCategoriesConfigJson);
        }

        public async Task SyncCaptainsCost()
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
    struct UserAttributes { }

    struct AppAttributes { }
}
