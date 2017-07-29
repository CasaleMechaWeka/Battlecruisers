using System.Collections.Generic;
using BattleCruisers.Data.Serialization;
using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
    // FELIX  Move to own file
    public interface IDataProvider
	{
		IList<ILevel> Levels { get; }
		GameModel GameModel { get; }
		int NumOfLevelsUnlocked { get; }
        IStaticData StaticData { get; }

		ILevel GetLevel(int levelNum);
		void SaveGame();
	}

	public class DataProvider : IDataProvider
	{
		private readonly ISerializer _serializer;
		
        public IStaticData StaticData { get; private set; }
		public IList<ILevel> Levels { get { return StaticData.Levels; } }
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

			StaticData = staticData;
			_serializer = serializer;

			if (_serializer.DoesSavedGameExist())
			{
				GameModel = _serializer.LoadGame();
			}
			else
			{
				GameModel = StaticData.InitialGameModel;
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

