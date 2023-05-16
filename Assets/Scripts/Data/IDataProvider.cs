using System.Collections.Generic;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;

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
        IPvPLevel GetPvPLevel(int levelNum);
        void SaveGame();

        /// <summary>
        /// Deletes the saved game and resets all settings to their default values.
        /// Designed for user playtests, so users can start with a clean slate.
        /// </summary>
        void Reset();
    }
}
