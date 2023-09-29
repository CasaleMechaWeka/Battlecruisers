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
            Loadout loadout = (Loadout)plo;

            if (loadout.CurrentCaptain == null)
            {
                // make GameModel as compatible as possible
                game = MakeCompatible(output);
            }
            else
            {
                // assign as was previously done
                game = (GameModel)output;
            }

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

            // What levels have been completed, and at what difficulty
            foreach (var level in gameData.GetType().GetProperty("CompletedLevels").GetValue(gameData) as IReadOnlyCollection<CompletedLevel>)
            {
                compatibleGameModel.AddCompletedLevel(level);
            }
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
            SaveGameModel saveData = new SaveGameModel(game);
            try
            {
                var data = new Dictionary<string, object> { { "GameModel", SerializeGameModel(saveData) } };
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            }
            catch (UnityException e)
            {
                Debug.LogException(e);
            }
        }

        public async Task<SaveGameModel> CloudLoad(GameModel game)
        {
            List<string> keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();
            if (keys.Contains("GameModel"))
            {
                try
                {
                    Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "GameModel" });
                    if (savedData != null && savedData["GameModel"] != String.Empty)
                    {
                        SaveGameModel saveModel = (SaveGameModel)DeserializeGameModel(savedData["GameModel"]);
                        Debug.Log(savedData["GameModel"]);

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
                    Debug.LogException(e);
                    return null;
                }
            }
            else
            {
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
                dataProvider.SaveGame();
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

        public async Task<bool> SyncInventoryFromCloud(IDataProvider dataProivder)
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
                        dataProivder.GameModel.Captains[index].isOwned = true;
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Heckle"))
                    {
                        int index = StaticPrefabKeys.HeckleItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProivder.GameModel.Heckles[index].isOwned = true;
                    }
                }
                dataProivder.SaveGame();
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
                        dataProivder.GameModel.Captains[index].isOwned = true;
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Heckle"))
                    {
                        int index = StaticPrefabKeys.HeckleItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProivder.GameModel.Heckles[index].isOwned = true;
                    }

                }
                dataProivder.SaveGame();
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
