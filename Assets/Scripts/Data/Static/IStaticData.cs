using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Data.Static
{
    /// <summary>
    /// Provides data that does not change throughout the game.
    /// 
    /// This is in contrast to the GameModel, which changes as the player
    /// progresses and unlocks new prefabs.
    /// 
    /// Heckles, Captains, Bodykits, Variants only have their prices adjusted
    /// at launch, so during gameplay this stays constant as well
    /// </summary>
    public interface IStaticData
    {
        GameModel InitialGameModel { get; }
        ReadOnlyCollection<ILevel> Levels { get; }
        ReadOnlyCollection<ISideQuestData> SideQuests { get; }
        ReadOnlyDictionary<Map, IPvPLevel> PvPLevels { get; }
        ReadOnlyCollection<HullKey> HullKeys { get; }
        ReadOnlyCollection<UnitKey> UnitKeys { get; }
        ReadOnlyCollection<BuildingKey> BuildingKeys { get; }
        ReadOnlyCollection<BuildingKey> AIBannedUltrakeys { get; }
        IReadOnlyList<HeckleData> Heckles { get; }
        IReadOnlyList<CaptainData> Captains { get; }
        IReadOnlyList<BodykitData> Bodykits { get; }
        IReadOnlyList<VariantData> Variants { get; }
        IReadOnlyList<IAPData> IAPs { get; }
        int LastLevelWithLoot { get; }
        ILevelStrategies Strategies { get; }
        ILevelStrategies SideQuestStrategies { get; }
        IPvPLevelStrategies PvPStrategies { get; }

        ILoot GetLevelLoot(int levelCompleted);
        ILoot GetSideQuestLoot(int sideQuestID);
        int UnitUnlockLevel(UnitKey unitKey);
        int BuildingUnlockLevel(BuildingKey buildingKey);
    }
}
