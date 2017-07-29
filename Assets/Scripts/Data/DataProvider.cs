using System.Collections.Generic;
using BattleCruisers.Data.Serialization;
using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
    public interface IDataProvider
	{
		IList<ILevel> Levels { get; }
		GameModel GameModel { get; }
		int NumOfLevelsUnlocked { get; }

		ILevel GetLevel(int levelNum);
		void SaveGame();
	}

	public class DataProvider : IDataProvider
	{
		private readonly IStaticData _staticData;
		private readonly ISerializer _serializer;

		public IList<ILevel> Levels { get { return _staticData.Levels; } }
		public GameModel GameModel { get; private set; }

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

		public ILevel GetLevel(int levelNum)
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

