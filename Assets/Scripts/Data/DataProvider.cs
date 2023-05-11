using System.Collections.Generic;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Serialization;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
    public class DataProvider : IDataProvider
	{
		private readonly ISerializer _serializer;
		
        public IStaticData StaticData { get; }
		public IList<ILevel> Levels => StaticData.Levels;
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

		public void SaveGame()
		{
            _serializer.SaveGame(_gameModel);
		}

        public void Reset()
        {
            _serializer.DeleteSavedGame();
        }
    }
}
