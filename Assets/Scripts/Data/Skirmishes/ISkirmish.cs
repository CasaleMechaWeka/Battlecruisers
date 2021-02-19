using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;

namespace BattleCruisers.Data.Skirmishes
{
    public interface ISkirmish
    {
        IPrefabKey AICruiser { get; }
        StrategyType AIStrategy { get; }
        Difficulty Difficulty { get; }
    }
}