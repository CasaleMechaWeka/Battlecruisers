using System.Collections.Generic;

namespace BattleCruisers.Data
{
    public interface IDataProvider
    {
        IList<ILevel> Levels { get; }
        GameModel GameModel { get; }
        int NumOfLevelsUnlocked { get; }
        IStaticData StaticData { get; }

        ILevel GetLevel(int levelNum);
        void SaveGame();
    }
}
	