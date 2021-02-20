using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;

namespace BattleCruisers.Data.Models
{
    public interface ISkirmishModel
    {
        HullKey AICruiser { get; }
        StrategyType AIStrategy { get; }
        Difficulty Difficulty { get; }
    }
}