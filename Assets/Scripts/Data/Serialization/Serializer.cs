using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using BattleCruisers.Data.Models;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using BattleCruisers.Utils.UGS.Samples;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Models.PrefabKeys;
using System.Linq;
using System.Reflection;
using BattleCruisers.UI.ScreensScene.ShopScreen;


namespace BattleCruisers.Data.Serialization
{
    public class Serializer : ISerializer
    {
        private readonly IModelFilePathProvider _modelFilePathProvider;
        private readonly BinaryFormatter _binaryFormatter;
        private const int NUM_OF_VARIANTS = 131;

        public Serializer(IModelFilePathProvider modelFilePathProvider)
        {
            _modelFilePathProvider = modelFilePathProvider;
            _binaryFormatter = new BinaryFormatter();
        }

        public bool DoesSavedGameExist()
        {
            return File.Exists(_modelFilePathProvider.GameModelFilePath);
        }

        public void SaveGame(GameModel game)
        {
            FileStream file = File.Create(_modelFilePathProvider.GameModelFilePath);
            _binaryFormatter.Serialize(file, game);
            file.Close();
        }

        public GameModel LoadGame()
        {
            Assert.IsTrue(DoesSavedGameExist());

            FileStream file = File.Open(_modelFilePathProvider.GameModelFilePath, FileMode.Open);

            object output = _binaryFormatter.Deserialize(file);
            GameModel game;

            // We need to track Save vs Install versions
            // since we don't do that right now, I'm just checking inside the Loadout to see whether the user has a captains set.
            // Not having a captain set causes the game to hang on the first load screen, so this is a good test now.
            // It should be changed to a version check though.
            var plo = output.GetType().GetProperty("PlayerLoadout").GetValue(output);
            var vts = output.GetType().GetProperty("Variants").GetValue(output);

            bool compatibleHeckles = false;
            try
            {
                var purchasedHecklesProperty = output.GetType().GetProperty("PurchasedHeckles");
                if (purchasedHecklesProperty != null)
                    if (purchasedHecklesProperty.GetValue(output) is List<int>)
                        compatibleHeckles = true;
                    else
                        Debug.LogWarning("Property \"PurchasedHeckles\" was not in the expected format: List<int>");
                else
                    Debug.Log("Property \"PurchasedHeckles\" was not found");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while reading \"PurchasedHeckles\" property from save data:\n" + ex.Message);
            }

            bool compatibleExos = false;
            try
            {
                var purchasedExosProperty = output.GetType().GetProperty("PurchasedExos");
                if (purchasedExosProperty != null)
                    if (purchasedExosProperty.GetValue(output) is List<int>)
                        compatibleExos = true;
                    else
                        Debug.LogWarning("Property \"PurchasedExos\" was not in the expected format: List<int>");
                else
                    Debug.Log("Property \"PurchasedExos\" was not found");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while reading \"PurchasedExos\" property from save data:\n" + ex.Message);
            }

            bool compatibleBodykits = false;
            try
            {
                var purchasedBodykitsProperty = output.GetType().GetProperty("PurchasedBodykits");
                if (purchasedBodykitsProperty != null)
                    if (purchasedBodykitsProperty.GetValue(output) is List<int>)
                        compatibleBodykits = true;
                    else
                        Debug.LogWarning("Property \"PurchasedBodykits\" was not in the expected format: List<int>");
                else
                    Debug.Log("Property \"PurchasedBodykits\" was not found");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while reading \"PurchasedBodykits\" property from save data:\n" + ex.Message);
            }


            Loadout loadout = (Loadout)plo;

            if (loadout.CurrentCaptain == null || loadout.SelectedVariants == null || vts == null || compatibleHeckles == false ||
            ((GameModel)output).Variants.Count < NUM_OF_VARIANTS || ((GameModel)output).NumOfLevelsCompleted > StaticData.NUM_OF_LEVELS ||
            compatibleExos == false || compatibleBodykits == false)
            {
                // make GameModel as compatible as possible
                game = MakeCompatible(output);
            }
            else
            {
                // assign as was previously done
                game = (GameModel)output;
            }

#if PREMIUM_EDITION
            game.PremiumEdition = true;
            game.AddBodykit(0);
#endif

            file.Close();
            return game;
        }

        private GameModel MakeCompatible(object gameData)
        {
            // vars
            var tut = gameData.GetType().GetProperty("HasAttemptedTutorial").GetValue(gameData);
            var lds = gameData.GetType().GetProperty("LifetimeDestructionScore").GetValue(gameData);
            var bds = gameData.GetType().GetProperty("BestDestructionScore").GetValue(gameData);
            var plo = gameData.GetType().GetProperty("PlayerLoadout").GetValue(gameData);
            var lbr = gameData.GetType().GetProperty("LastBattleResult").GetValue(gameData);
            var pre = gameData.GetType().GetProperty("PremiumEdition").GetValue(gameData);

            List<HullKey> _unlockedHulls = new List<HullKey>();
            foreach (var hull in gameData.GetType().GetProperty("UnlockedHulls").GetValue(gameData) as IReadOnlyCollection<HullKey>)
            {
                _unlockedHulls.Add(hull);
            }

            List<BuildingKey> _unlockedBuildings = new List<BuildingKey>();
            foreach (var building in gameData.GetType().GetProperty("UnlockedBuildings").GetValue(gameData) as IReadOnlyCollection<BuildingKey>)
            {
                _unlockedBuildings.Add(building);
            }

            List<UnitKey> _unlockedUnits = new List<UnitKey>();
            foreach (var unit in gameData.GetType().GetProperty("UnlockedUnits").GetValue(gameData) as IReadOnlyCollection<UnitKey>)
            {
                _unlockedUnits.Add(unit);
            }

            // compiler doesn't like them being cast when they're assigned, so they're cast here
            bool _hasAttemptedTutorial = (bool)tut;
            long _lifetimeDestructionScore = (long)lds;
            long _bestDestructionScore = (long)bds;
            Loadout _playerLoadout = (Loadout)plo;
            BattleResult _lastBattleResult = (BattleResult)lbr;
            bool _premiumState = (bool)pre;

            bool _hasSyncdShop = false;

            // GameModel gets constructed from the fields we've pulled out of gameData:
            GameModel compatibleGameModel = new GameModel(
                _hasSyncdShop,
                _hasAttemptedTutorial,
                _lifetimeDestructionScore,
                _bestDestructionScore,
                _playerLoadout,
                _lastBattleResult,
                _unlockedHulls,
                _unlockedBuildings,
                _unlockedUnits
                );

            compatibleGameModel.PremiumEdition = _premiumState;

            // ##############################################
            //                  New Fields
            // ##############################################

            // Selected Captain
            if (_playerLoadout.CurrentCaptain == null)
            {
                compatibleGameModel.PlayerLoadout.PurchaseExo("CaptainExo000");
                compatibleGameModel.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            }

            // Heckles
            if (_playerLoadout.CurrentHeckles == null)
            {
                compatibleGameModel.PlayerLoadout.CurrentHeckles = new List<int> { 0, 1, 2 };
            }

            PropertyInfo[] properties = gameData.GetType().GetProperties();
            //foreach (PropertyInfo propertyInfo in properties)
            //    Debug.Log(propertyInfo.Name);

            try
            {
                var purchasedHecklesProperty = gameData.GetType().GetProperty("PurchasedHeckles");
                if (purchasedHecklesProperty != null)
                {
                    if (purchasedHecklesProperty.GetValue(gameData) is List<int> purchasedHeckles && purchasedHeckles.Count > 0)
                        foreach (int i in purchasedHeckles)
                            compatibleGameModel.AddHeckle(purchasedHeckles[i]);
                    else
                        Debug.LogError("Property \"PurchasedHeckles\" was not in the expected format List<int>");
                }
                else
                    Debug.LogWarning("Property \"PurchasedHeckles\" is null in save data");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error when processing \"PurchasedHeckles\": " + ex.Message);
            }

            try
            {
                var purchasedExosProperty = gameData.GetType().GetProperty("PurchasedExos");
                if (purchasedExosProperty != null)
                {
                    if (purchasedExosProperty.GetValue(gameData) is List<int> purchasedExos && purchasedExos.Count > 0)
                        foreach (int i in purchasedExos)
                            compatibleGameModel.AddExo(purchasedExos[i]);
                    else
                        Debug.LogError("Property \"PurchasedExos\" was not in the expected format List<int>");
                }
                else
                    Debug.LogWarning("Property \"PurchasedExos\" is null in save data");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error when processing \"PurchasedExos\": " + ex.Message);
            }

            try
            {
                var purchasedBodykitsProperty = gameData.GetType().GetProperty("PurchasedBodykits");
                if (purchasedBodykitsProperty != null)
                {
                    if (purchasedBodykitsProperty.GetValue(gameData) is List<int> purchasedBodykits && purchasedBodykits.Count > 0)
                        foreach (int i in purchasedBodykits)
                            compatibleGameModel.AddExo(purchasedBodykits[i]);
                    else
                        Debug.LogError("Property \"PurchasedBodykits\" was not in the expected format List<int>");
                }
                else
                    Debug.LogWarning("Property \"PurchasedBodykits\" is null in save data");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error when processing \"PurchasedBodykits\": " + ex.Message);
            }

            var exos = gameData.GetType().GetProperty("Captains").GetValue(gameData) as IList<object>;
            if (exos != null)
                for (int i = 0; i < exos.Count; i++)
                {
                    var exo = exos[i];
                    if (exo == null) continue;
                    var isOwnedProperty = exo.GetType().GetProperty("isOwned");
                    if (isOwnedProperty == null) continue;
                    var indexProperty = exo.GetType().GetProperty("index");
                    if (indexProperty == null) continue;
                    if (isOwnedProperty.GetValue(exo) is bool isOwned && isOwned
                        && indexProperty.GetValue(exo) is int index)
                        compatibleGameModel.AddExo(index);
                }

            var heckles = gameData.GetType().GetProperty("Heckles").GetValue(gameData) as IList<object>;
            if (heckles != null)
                for (int i = 0; i < heckles.Count; i++)
                {
                    var heckle = heckles[i];
                    if (heckle == null) continue;
                    var isOwnedProperty = heckle.GetType().GetProperty("isOwned");
                    if (isOwnedProperty == null) continue;
                    var indexProperty = heckle.GetType().GetProperty("index");
                    if (indexProperty == null) continue;
                    if (isOwnedProperty.GetValue(heckle) is bool isOwned && isOwned
                        && indexProperty.GetValue(heckle) is int index)
                        compatibleGameModel.AddHeckle(index);
                }

            var bodykits = gameData.GetType().GetProperty("Bodykits").GetValue(gameData) as IList<object>;
            if (bodykits != null)
                for (int i = 0; i < bodykits.Count; i++)
                {
                    var bodykit = bodykits[i];
                    if (bodykit == null) continue;
                    var isOwnedProperty = bodykit.GetType().GetProperty("isOwned");
                    if (isOwnedProperty == null) continue;
                    var indexProperty = bodykit.GetType().GetProperty("index");
                    if (indexProperty == null) continue;
                    if (isOwnedProperty.GetValue(bodykit) is bool isOwned && isOwned
                        && indexProperty.GetValue(bodykit) is int index)
                        compatibleGameModel.AddBodykit(index);
                }

            if (gameData.GetType().GetProperty("Variants").GetValue(gameData) != null && (gameData.GetType().GetProperty("Variants").GetValue(gameData) as IReadOnlyCollection<VariantData>).Count != 0)
                foreach (VariantData variant in gameData.GetType().GetProperty("Variants").GetValue(gameData) as IReadOnlyCollection<VariantData>)
                    if (variant.IsOwned)
                    {
                        compatibleGameModel.AddVariant(variant.index);
                        compatibleGameModel._variants[variant.index].isOwned = true;
                    }

            if (gameData.GetType().GetProperty("BattleWinScore").GetValue(gameData) != null)
                compatibleGameModel.BattleWinScore = (float)gameData.GetType().GetProperty("BattleWinScore").GetValue(gameData);

            if (gameData.GetType().GetProperty("Coins").GetValue(gameData) != null)
                compatibleGameModel.Coins = (long)gameData.GetType().GetProperty("Coins").GetValue(gameData);

            if (gameData.GetType().GetProperty("Credits").GetValue(gameData) != null)
                compatibleGameModel.Credits = (long)gameData.GetType().GetProperty("Credits").GetValue(gameData);

            if (gameData.GetType().GetProperty("CoinsChange").GetValue(gameData) != null)
                compatibleGameModel.CoinsChange = (int)gameData.GetType().GetProperty("CoinsChange").GetValue(gameData);

            if (gameData.GetType().GetProperty("CreditsChange").GetValue(gameData) != null)
                compatibleGameModel.CreditsChange = (int)gameData.GetType().GetProperty("CreditsChange").GetValue(gameData);

            // Variants
            if (_playerLoadout.SelectedVariants == null)
            {
                _playerLoadout.SelectedVariants = new List<int>();
            }

            // Player Name
            string _playerName = gameData.GetType().GetProperty("PlayerName").GetValue(gameData) as string;
            if (_playerName == null || _playerName == "")
            {
                compatibleGameModel.PlayerName = "Charlie";
            }
            else
            {
                compatibleGameModel.PlayerName = _playerName;
            }

            IReadOnlyCollection<CompletedLevel> completedLevels = gameData.GetType().GetProperty("CompletedLevels").GetValue(gameData) as IReadOnlyCollection<CompletedLevel>;

            int completedLevelsCount = Mathf.Min(StaticData.NUM_OF_LEVELS, completedLevels.Count);

            // What levels have been completed, and at what difficulty
            for (int i = 0; i < completedLevelsCount; i++)
            {
                compatibleGameModel.AddCompletedLevel(completedLevels.ElementAt(i));
            }

            List<int> completedSideQuestIDs = compatibleGameModel.CompletedSideQuests.Select(data => data.LevelNum).ToList();

            //needs to be hardcoded since otherwise access to StaticData.cs would be required
            //update this whenever loot unlock requirements are modified 

            if (_unlockedBuildings.Contains(StaticPrefabKeys.Buildings.NovaArtillery) && !completedSideQuestIDs.Contains(0))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(0, Settings.Difficulty.Hard));
            if (_unlockedHulls.Contains(StaticPrefabKeys.Hulls.Rickshaw) && !completedSideQuestIDs.Contains(1))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(1, Settings.Difficulty.Hard));
            if (_unlockedHulls.Contains(StaticPrefabKeys.Hulls.TasDevil) && !completedSideQuestIDs.Contains(2))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(2, Settings.Difficulty.Hard));
            if (_unlockedBuildings.Contains(StaticPrefabKeys.Buildings.MissilePod) && !completedSideQuestIDs.Contains(3))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(3, Settings.Difficulty.Hard));
            if (_unlockedHulls.Contains(StaticPrefabKeys.Hulls.BlackRig) && !completedSideQuestIDs.Contains(4))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(4, Settings.Difficulty.Hard));
            if (_unlockedBuildings.Contains(StaticPrefabKeys.Buildings.IonCannon) && !completedSideQuestIDs.Contains(5))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(5, Settings.Difficulty.Hard));
            if (_unlockedBuildings.Contains(StaticPrefabKeys.Buildings.Coastguard) && !completedSideQuestIDs.Contains(6))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(6, Settings.Difficulty.Hard));
            if (_unlockedHulls.Contains(StaticPrefabKeys.Hulls.Yeti) && !completedSideQuestIDs.Contains(7))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(7, Settings.Difficulty.Hard));
            if (_unlockedUnits.Contains(StaticPrefabKeys.Units.Broadsword) && !completedSideQuestIDs.Contains(8))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(8, Settings.Difficulty.Hard));

            if (compatibleGameModel.CompletedLevels != null && compatibleGameModel.CompletedLevels.Count > 0)
            {
                compatibleGameModel.HasAttemptedTutorial = true;
            }

            return compatibleGameModel;
        }

        public void DeleteSavedGame()
        {
            if (DoesSavedGameExist())
            {
                File.Delete(_modelFilePathProvider.GameModelFilePath);
            }
        }

        public object DeserializeGameModel(string gameModelJSON)
        {
            return JsonConvert.DeserializeObject<SaveGameModel>(gameModelJSON);
        }

        public string SerializeGameModel(object saveGameModel)
        {
            return JsonConvert.SerializeObject(saveGameModel); //, new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});
            // ^ uncomment this if serializing vectors
        }

        public async Task CloudSave(GameModel game)
        {
            try
            {
                SaveGameModel saveData = new SaveGameModel(game);
                if (CloudSaveService.Instance != null && CloudSaveService.Instance.Data != null)
                {
                    var serializedData = SerializeGameModel(saveData);
                    if (!string.IsNullOrEmpty(serializedData))
                    {
                        var data = new Dictionary<string, object> { { "GameModel", serializedData } };
                        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
                    }
                    else
                    {
                        Debug.LogError("CloudSave Error: Serialized data is empty or null.");
                    }
                }
                else
                {
                    Debug.LogError("CloudSave Error: CloudSaveService instance or Data is null.");
                }
            }
            catch (TimeoutException e)
            {
                Debug.LogWarning("Timeout occurred: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("CloudSave Error: " + e);
            }
        }

        public async Task<SaveGameModel> CloudLoad(GameModel game)
        {
            try
            {
                Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "GameModel" });
                if (savedData != null && savedData.TryGetValue("GameModel", out string gameModelData) && !string.IsNullOrEmpty(gameModelData))
                {
                    SaveGameModel saveModel = (SaveGameModel)DeserializeGameModel(gameModelData);
                    Debug.Log(gameModelData);

                    //saveModel.AssignSaveToGameModel(game); <-- Moved to CloudLoad() method in DataProvider
                    if (saveModel._lifetimeDestructionScore >= game.LifetimeDestructionScore)
                    {
                        Debug.Log("Cloud save up to date");
                        return saveModel;
                    }
                    else
                    {
                        Debug.Log("Cloud save not up to date");
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (UnityException e)
            {
                Debug.LogError("CloudLoad Error: " + e);
                return null;
            }
            catch (TimeoutException e)
            {
                Debug.LogWarning("CloudLoad Timeout Occurred: " + e);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError("CloudLoad Error: " + e);
                return null;
            }
        }

        public async void DeleteCloudSave()
        {
            await CloudSaveService.Instance.Data.ForceDeleteAsync("GameModel");
        }

        public async Task<bool> SyncCoinsToCloud(IDataProvider dataProvider)
        {
            try
            {
                await EconomyManager.SetEconomyBalance("COIN", dataProvider.GameModel.Coins);
                return true;
            }
            catch (EconomyRateLimitedException e)
            {
                Debug.LogException(e);
                return false;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }

        public async Task<bool> SyncCurrencyFromCloud(IDataProvider dataProvider)
        {
            GetBalancesResult balanceResult = null;
            try
            {
                if (this == null) return false;
                balanceResult = await EconomyManager.GetEconomyBalances();
                if (this == null) return false;
                if (balanceResult is null) return false;
                foreach (var balance in balanceResult.Balances)
                {
                    if (balance.Balance > 0 && balance.CurrencyId == "COIN")
                    {
                        dataProvider.GameModel.Coins = balance.Balance;
                    }
                    if (balance.Balance > 0 && balance.CurrencyId == "CREDIT")
                    {
                        dataProvider.GameModel.Credits = balance.Balance;
                    }
                }
                return true;
            }
            catch (EconomyRateLimitedException e)
            {
                balanceResult = await BattleCruisers.Utils.UGS.Samples.Utils.RetryEconomyFunction(EconomyManager.GetEconomyBalances, e.RetryAfter);
                if (this == null) return false;
                if (balanceResult is null) return false;
                foreach (var balance in balanceResult.Balances)
                {
                    if (balance.Balance > 0 && balance.CurrencyId == "COIN")
                    {
                        dataProvider.GameModel.Coins = balance.Balance;
                    }
                    if (balance.Balance > 0 && balance.CurrencyId == "CREDIT")
                    {
                        dataProvider.GameModel.Credits = balance.Balance;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }

        public async Task<bool> SyncInventoryFromCloud(IDataProvider dataProvider)
        {
            GetInventoryResult inventoryResult = null;
            try
            {
                inventoryResult = await EconomyManager.GetEconomyInventories();
                if (this == null) return false;
                if (inventoryResult is null) return false;
                foreach (var inventory in inventoryResult.PlayersInventoryItems)
                {
                    if (inventory.GetItemDefinition().Name.Contains("Captain"))
                    {
                        int index = StaticPrefabKeys.CaptainItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProvider.GameModel.AddExo(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Heckle"))
                    {
                        int index = StaticPrefabKeys.HeckleItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProvider.GameModel.AddHeckle(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Bodykit"))
                    {
                        int index = StaticPrefabKeys.BodykitItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProvider.GameModel.AddBodykit(index);
                    }
                }
                dataProvider.SaveGame();
                return true;
            }
            catch (EconomyRateLimitedException e)
            {
                inventoryResult = await BattleCruisers.Utils.UGS.Samples.Utils.RetryEconomyFunction(EconomyManager.GetEconomyInventories, e.RetryAfter);
                if (this == null) return false;
                if (inventoryResult is null) return false;
                foreach (var inventory in inventoryResult.PlayersInventoryItems)
                {
                    if (inventory.GetItemDefinition().Name.Contains("Captain"))
                    {
                        int index = StaticPrefabKeys.CaptainItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProvider.GameModel.AddExo(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Heckle"))
                    {
                        int index = StaticPrefabKeys.HeckleItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProvider.GameModel.AddHeckle(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Bodykit"))
                    {
                        int index = StaticPrefabKeys.BodykitItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProvider.GameModel.AddBodykit(index);
                    }
                }
                dataProvider.SaveGame();
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }

        public async Task<bool> SyncCreditsToCloud(IDataProvider dataProvider)
        {
            try
            {
                await EconomyManager.SetEconomyBalance("CREDIT", dataProvider.GameModel.Credits);
                return true;
            }
            catch (EconomyRateLimitedException e)
            {
                Debug.LogException(e);
                return false;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }
    }
}
