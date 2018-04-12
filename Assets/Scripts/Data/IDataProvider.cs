using System.Collections.Generic;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Data
{
    public interface IDataProvider
    {
        IList<ILevel> Levels { get; }
        IGameModel GameModel { get; }
        ILockedInformation LockedInfo { get; }
        IStaticData StaticData { get; }
        ISettingsManager SettingsManager { get; }

        ILevel GetLevel(int levelNum);
        void SaveGame();
    }
}
	