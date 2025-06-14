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
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes;

namespace BattleCruisers.Data.Serialization
{
    public class Serializer : ISerializer
    {
        private readonly IModelFilePathProvider _modelFilePathProvider;
        private readonly BinaryFormatter _binaryFormatter;

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
            using (FileStream file = File.Create(_modelFilePathProvider.GameModelFilePath))
            {
                _binaryFormatter.Serialize(file, game);

                file.Close();
            }
        }

        public GameModel LoadGame()
        {
            Assert.IsTrue(DoesSavedGameExist());
            object output = null;
            using (FileStream file = File.Open(_modelFilePathProvider.GameModelFilePath, FileMode.Open))
            {
                _binaryFormatter.Deserialize(file);
                file.Close();
            }

            GameModel game;
            if (output == null)
            {
                LandingSceneGod.Instance.LogToScreen("output == null");
                game = StaticData.InitialGameModel;
            }

            // We need to track Save vs Install versions
            // since we don't do that right now, I'm just checking inside the Loadout to see whether the user has a captains set.
            // Not having a captain set causes the game to hang on the first load screen, so this is a good test now.
            // It should be changed to a version check though.
            var plo = output.GetType().GetProperty("PlayerLoadout").GetValue(output);

            string[] purchasableCategories = new string[]
            {
                "Heckles", "Exos", "Bodykits", "Variants"
            };

            byte compatiblePurchasables = 0;

            foreach (string purchasableCategory in purchasableCategories)
                try
                {
                    var purchasableProperty = output.GetType().GetProperty("Purchased" + purchasableCategory);
                    if (purchasableProperty != null)
                        if (purchasableProperty.GetValue(output) is List<int>)
                            compatiblePurchasables++;
                        else
                            Debug.LogWarning("Property \"Purchased" + purchasableCategory + "\" was not in the expected format: List<int>");
                    else
                        Debug.Log("Property \"Purchased" + purchasableCategory + "\" was not found");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error while reading \"Purchased" + purchasableCategory + "\" property from save data:\n" + ex.Message);
                }

            Loadout loadout = (Loadout)plo;

            bool validLoadoutCatrgories = true;

            if (loadout.SelectedBuildings[BuildingCategory.Factory].Count > 5 ||
               loadout.SelectedBuildings[BuildingCategory.Defence].Count > 5 ||
               loadout.SelectedBuildings[BuildingCategory.Offence].Count > 5 ||
               loadout.SelectedBuildings[BuildingCategory.Ultra].Count > 5 ||
               loadout.SelectedUnits[UnitCategory.Naval].Count > 5 ||
               loadout.SelectedUnits[UnitCategory.Aircraft].Count > 5)
            {
                validLoadoutCatrgories = false;
            }

            if (loadout.CurrentCaptain == null || loadout.SelectedVariants == null || !validLoadoutCatrgories || compatiblePurchasables != purchasableCategories.Length ||
                ((GameModel)output).NumOfLevelsCompleted > StaticData.NUM_OF_LEVELS)
            {
                // make GameModel as compatible as possible
                game = MakeCompatible(output);
            }
            else
            {
                // assign as was previously done
                game = (GameModel)output;
            }

            // If any variant is in SelectedVariants but missing from PurchasedVariants, restore it
            if (game.PlayerLoadout.SelectedVariants != null && game.PlayerLoadout.SelectedVariants.Count > 0)
            {
                int restoredCount = 0;
                foreach (int selectedVariantId in game.PlayerLoadout.SelectedVariants)
                {
                    if (!game.PurchasedVariants.Contains(selectedVariantId))
                    {
                        game.AddVariant(selectedVariantId);
                        restoredCount++;
                        Debug.Log($"RECOVERY: Restored missing purchased variant {selectedVariantId} (found in SelectedVariants)");
                    }
                }

                if (restoredCount > 0)
                {
                    Debug.Log($"RECOVERY: Successfully restored {restoredCount} missing purchased variants from SelectedVariants");
                }
            }

#if PREMIUM_EDITION
            game.PremiumEdition = true;
            game.AddBodykit(0);
#endif

            return game;
        }

        private GameModel MakeCompatible(object gameData)
        {
            Debug.Log("MakeCompatible");
            // vars
            var tut = gameData.GetType().GetProperty("HasAttemptedTutorial").GetValue(gameData);
            var lds = gameData.GetType().GetProperty("LifetimeDestructionScore").GetValue(gameData);
            var bds = gameData.GetType().GetProperty("BestDestructionScore").GetValue(gameData);
            var plo = gameData.GetType().GetProperty("PlayerLoadout").GetValue(gameData);
            var lbr = gameData.GetType().GetProperty("LastBattleResult").GetValue(gameData);
            var pre = gameData.GetType().GetProperty("PremiumEdition").GetValue(gameData);

            List<HullKey> _unlockedHulls = new List<HullKey>();
            foreach (var hull in gameData.GetType().GetProperty("UnlockedHulls").GetValue(gameData) as IReadOnlyCollection<HullKey>)
                _unlockedHulls.Add(hull);

            List<BuildingKey> _unlockedBuildings = new List<BuildingKey>();
            foreach (var building in gameData.GetType().GetProperty("UnlockedBuildings").GetValue(gameData) as IReadOnlyCollection<BuildingKey>)
                _unlockedBuildings.Add(building);

            List<UnitKey> _unlockedUnits = new List<UnitKey>();
            foreach (var unit in gameData.GetType().GetProperty("UnlockedUnits").GetValue(gameData) as IReadOnlyCollection<UnitKey>)
                _unlockedUnits.Add(unit);

            // compiler doesn't like them being cast when they're assigned, so they're cast here
            bool _hasAttemptedTutorial = (bool)tut;
            long _lifetimeDestructionScore = (long)lds;
            long _bestDestructionScore = (long)bds;
            Loadout _loadout = (Loadout)plo;
            _loadout.ValidateSelectedBuildables();

            BattleResult _lastBattleResult = (BattleResult)lbr;
            bool _premiumState = (bool)pre;

            bool _hasSyncdShop = false;

            // GameModel gets constructed from the fields we've pulled out of gameData:
            GameModel compatibleGameModel = new GameModel(
                _hasSyncdShop,
                _hasAttemptedTutorial,
                _lifetimeDestructionScore,
                _bestDestructionScore,
                _loadout,
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
            if (_loadout.CurrentCaptain == null)
            {
                compatibleGameModel.PlayerLoadout.PurchaseExo("CaptainExo000");
                compatibleGameModel.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            }

            // Heckles
            if (_loadout.CurrentHeckles == null)
            {
                compatibleGameModel.PlayerLoadout.CurrentHeckles = new List<int> { 0, 1, 2 };
            }

            PropertyInfo[] properties = gameData.GetType().GetProperties();
            //foreach (PropertyInfo propertyInfo in properties)
            //    Debug.Log(propertyInfo.Name);

            string[] purchasableCategories = new string[]
            {
                "Heckles", "Exos", "Bodykits", "Variants"
            };

            Action<int>[] purchasableOperations = new Action<int>[]
            {
                compatibleGameModel.AddHeckle,
                compatibleGameModel.AddExo,
                compatibleGameModel.AddBodykit,
                compatibleGameModel.AddVariant
            };

            for (int i = 0; i < purchasableCategories.Length; i++)
                try
                {
                    var purchasableProperty = gameData.GetType().GetProperty("Purchased" + purchasableCategories[i]);
                    if (purchasableProperty != null)
                    {
                        if (purchasableProperty.GetValue(gameData) is List<int> purchasableItems && purchasableItems.Count > 0)
                            foreach (int j in purchasableItems)
                                purchasableOperations[i](j);
                        else
                            Debug.LogError("Property \"Purchased" + purchasableCategories[i] + "\" was not in the expected format List<int>");
                    }
                    else
                        Debug.LogWarning("Property \"Purchased" + purchasableCategories[i] + "\" is null in save data");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error when processing  \"Purchased" + purchasableCategories[i] + "\": " + ex.Message);
                }

            string[] purchasableCategoriesLegacy = new string[]
            {
                "Heckles", "Captains", "Bodykits", "Variants"
            };

            for (int i = 0; i < purchasableCategoriesLegacy.Length; i++)
                try
                {
                    var purchasableProperty = gameData.GetType().GetProperty(purchasableCategoriesLegacy[i]);
                    if (purchasableProperty != null)
                        if (purchasableProperty.GetValue(gameData) is List<int> purchasableItems && purchasableItems.Count > 0)
                            foreach (int? j in purchasableItems)
                            {
                                if (j == null) continue;
                                var isOwnedProperty = j.GetType().GetProperty("isOwned");
                                if (isOwnedProperty == null) continue;
                                var indexProperty = j.GetType().GetProperty("index");
                                if (indexProperty == null) continue;
                                if (isOwnedProperty.GetValue(j) is bool isOwned && isOwned
                                && indexProperty.GetValue(j) is int index)
                                    purchasableOperations[i](index);
                            }
                        else
                            Debug.LogError("Property \"Purchased" + purchasableCategoriesLegacy[i] + "\" was not in the expected format List<int>");
                    else
                        Debug.LogWarning("Property \"Purchased" + purchasableCategoriesLegacy[i] + "\" is null in save data");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error when processing  \"Purchased" + purchasableCategoriesLegacy[i] + "\": " + ex.Message);
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

            if (gameData.GetType().GetProperty("TimesLostOnLastLevel").GetValue(gameData) != null)
            {
                compatibleGameModel.TimesLostOnLastLevel = (int)gameData.GetType().GetProperty("TimesLostOnLastLevel").GetValue(gameData);
            }

            // Variants
            if (_loadout.SelectedVariants == null)
                _loadout.SelectedVariants = new List<int>();

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
                compatibleGameModel.HasAttemptedTutorial = true;

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

        public async Task<bool> SyncCoinsToCloud()
        {
            try
            {
                await EconomyManager.SetEconomyBalance("COIN", DataProvider.GameModel.Coins);
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

        public async Task<bool> SyncCurrencyFromCloud()
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
                        DataProvider.GameModel.Coins = balance.Balance;
                    }
                    if (balance.Balance > 0 && balance.CurrencyId == "CREDIT")
                    {
                        DataProvider.GameModel.Credits = balance.Balance;
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
                        DataProvider.GameModel.Coins = balance.Balance;
                    }
                    if (balance.Balance > 0 && balance.CurrencyId == "CREDIT")
                    {
                        DataProvider.GameModel.Credits = balance.Balance;
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

        public async Task<bool> SyncInventoryFromCloud()
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
                        DataProvider.GameModel.AddExo(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Heckle"))
                    {
                        int index = StaticPrefabKeys.HeckleItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddHeckle(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Bodykit"))
                    {
                        int index = StaticPrefabKeys.BodykitItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddBodykit(index);
                    }
                }
                DataProvider.SaveGame();
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
                        DataProvider.GameModel.AddExo(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Heckle"))
                    {
                        int index = StaticPrefabKeys.HeckleItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddHeckle(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Bodykit"))
                    {
                        int index = StaticPrefabKeys.BodykitItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddBodykit(index);
                    }
                }
                DataProvider.SaveGame();
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }

        public async Task<bool> SyncCreditsToCloud()
        {
            try
            {
                await EconomyManager.SetEconomyBalance("CREDIT", DataProvider.GameModel.Credits);
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
