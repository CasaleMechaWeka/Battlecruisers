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
        }

        public async Task CloudSave()
        {
            await _serializer.CloudSave(_gameModel);
        }

        public async Task CloudLoad()
        {
            await _serializer.CloudLoad();
        }

        public async Task SyncCoinsFromCloud()
        {
            await _serializer.SyncCoinsFromCloud(this);
        }

        public async Task SyncCoinsToCloud()
        {
            await _serializer.SyncCoinsToCloud(this);
        }

        public async Task SyncCreditsFromCloud()
        {
            await _serializer.SyncCreditsFromCloud(this);
        }

        public async Task SyncCreditsToCloud()
        {
            await _serializer.SyncCreditsToCloud(this);
        }
    }
}
