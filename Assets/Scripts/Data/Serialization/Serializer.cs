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
using System.Reflection;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using BattleCruisers.Utils.UGS.Samples;
using UnityEditor.Build;
using BattleCruisers.Data.Static;

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

            GameModel game = (GameModel)_binaryFormatter.Deserialize(file);
            file.Close();
            return game;
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
            return JsonConvert.DeserializeObject<GameModel>(gameModelJSON);
        }

        public string SerializeGameModel(object gameModel)
        {
            return JsonConvert.SerializeObject(gameModel, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        // Saves every local property of GameModel
        public async Task CloudSave(GameModel game)
        {
            foreach (var property in typeof(GameModel).GetProperties())
            {
                try
                {
                    string propertyName = property.Name;
                    object propertyValue = property.GetValue(game); //SerializeProperty(property.GetValue(game));

                    var data = new Dictionary<string, object> { { propertyName, propertyValue } };
                    await CloudSaveService.Instance.Data.ForceSaveAsync(data);
                }
                catch (UnityException e)
                {
                    Debug.LogException(e);
                }
            }
        }

        // Loads every cloud property of GameModel
        public async Task<GameModel> CloudLoad(GameModel _game)
        {
            // This method now requires a GameModel passed in.
            // This is so that it overwrites existing properties and leaves the rest, instead of creating null ones.
            GameModel game = _game;

            List<string> keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

            for (int i = 0; i <= keys.Count - 1; i++)
            {
                try
                {
                    string name = keys[i];
                    Dictionary<string, string> savedProperty = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { name });
                    var value = savedProperty[name];

                    PropertyInfo propertyInfo = game.GetType().GetProperty(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    propertyInfo.SetValue(game, value);
                }
                catch (UnityException e)
                {
                    Debug.LogException(e);
                }
            }
            return game;
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
                balanceResult = await EconomyManager.GetEconomyBalances();
                if (this == null) return false;
                if (balanceResult is null) return false;
                foreach (var balance in balanceResult.Balances)
                {
                    if (balance.Balance > 0 && balance.CurrencyId == "COIN")
                    {
                        dataProvider.GameModel.Coins = balance.Balance;
                    }
                    if(balance.Balance > 0 && balance.CurrencyId == "CREDIT")
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
                    if(inventory.GetItemDefinition().Name.Contains("Captain"))
                    {
                        int index = StaticPrefabKeys.CaptainItems[inventory.GetItemDefinition().Name.ToUpper()];
                        dataProivder.GameModel.Captains[index].isOwned = true;
                    }
                    if(inventory.GetItemDefinition().Name.Contains("Heckle"))
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
