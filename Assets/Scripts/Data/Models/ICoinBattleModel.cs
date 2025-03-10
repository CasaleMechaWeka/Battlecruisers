using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;

namespace BattleCruisers.Data.Models
{
    public interface ICoinBattleModel
    {
        HullKey PlayerCruiser { get; }
        Difficulty Difficulty { get; }
    }
}