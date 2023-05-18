using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Collections.ObjectModel;

namespace BattleCruisers.Data.Static
{
    /// <summary>
    /// Provides data that does not change throughout the game.
    /// 
    /// This is in contrast to the GameModel, which changes as the player
    /// progresses and unlocks new prefabs.
    /// </summary>
    public interface IStaticData
    {
        bool IsDemo { get; }
        bool HasAsserts { get; }
        GameModel InitialGameModel { get; }
        ReadOnlyCollection<ILevel> Levels { get; }
        ReadOnlyDictionary<Map, IPvPLevel> PvPLevels { get; }
        ReadOnlyCollection<HullKey> HullKeys { get; }
        ReadOnlyCollection<UnitKey> UnitKeys { get; }
        ReadOnlyCollection<BuildingKey> BuildingKeys { get; }
        ReadOnlyCollection<BuildingKey> AIBannedUltrakeys { get; }
        int LastLevelWithLoot { get; }
        ILevelStrategies Strategies { get; }

        ILoot GetLevelLoot(int levelCompleted);
        int LevelFirstAvailableIn(UnitKey unitKey);
        int LevelFirstAvailableIn(BuildingKey buildingKey);

    }
}
