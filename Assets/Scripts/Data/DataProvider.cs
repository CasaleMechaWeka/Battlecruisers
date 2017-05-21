using BattleCruisers.Data.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
	public interface IDataProvider
	{
		IList<Level> Levels { get; }
		GameModel GameModel { get; }

		Level GetLevel(int levelNum);
		void SaveGame();
	}

	public class DataProvider : IDataProvider
	{
		private readonly IStaticData _staticData;
		private readonly ISerializer _serializer;

		public IList<Level> Levels { get { return _staticData.Levels; } }
		public GameModel GameModel { get; private set; }

		public DataProvider(IStaticData staticData, ISerializer serializer)
		{
			Assert.IsNotNull(staticData);
			Assert.IsNotNull(serializer);

			_staticData = staticData;
			_serializer = serializer;

			if (_serializer.DoesSavedGameExist())
			{
				GameModel = _serializer.LoadGame();
			}
			else
			{
				GameModel = _staticData.InitialGameModel;
			}
		}

		public Level GetLevel(int levelNum)
		{
			Assert.IsTrue(levelNum > 0 && levelNum <= Levels.Count);
			return Levels[levelNum - 1];
		}

		public void SaveGame()
		{
			_serializer.SaveGame(GameModel);
		}
	}
}

