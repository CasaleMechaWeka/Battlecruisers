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
		
        public IStaticData StaticData { get; private set; }
		public IList<ILevel> Levels { get { return StaticData.Levels; } }
        public ISettingsManager SettingsManager { get; private set; }

        private readonly GameModel _gameModel;
        public IGameModel GameModel { get { return _gameModel; } }

		public int NumOfLevelsUnlocked
		{
			get
			{
				int numOfLevelsUnlocked = GameModel.NumOfLevelsCompleted + 1;
				if (numOfLevelsUnlocked > Levels.Count)
				{
					numOfLevelsUnlocked = Levels.Count;
				}
				return numOfLevelsUnlocked;
			}
		}

        public DataProvider(IStaticData staticData, ISerializer serializer, ISettingsManager settingsManager)
		{
            Helper.AssertIsNotNull(staticData, serializer, settingsManager);

			StaticData = staticData;
			_serializer = serializer;
            SettingsManager = settingsManager;

			if (_serializer.DoesSavedGameExist())
			{
                _gameModel = _serializer.LoadGame();
            }
            else
            {
                // First time run
                _gameModel = StaticData.InitialGameModel;
                SaveGame();
				
                SettingsManager.AIDifficulty = Difficulty.Normal;
                SettingsManager.Save();
			}
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
	}
}
