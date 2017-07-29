using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.Data
{
    /// <summary>
    /// Provides data that does not change throughout the game.
    /// 
    /// This is in contrast to the GameModel, which changes as the player
    /// progresses and unlocks new prefabs.
    /// </summary>
    public interface IStaticData
    {
        GameModel InitialGameModel { get; }
        IList<ILevel> Levels { get; }
        ReadOnlyCollection<IPrefabKey> BuildingKeys { get; }
    }
}
	