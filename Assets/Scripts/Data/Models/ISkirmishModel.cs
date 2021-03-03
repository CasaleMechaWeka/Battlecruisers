using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;

namespace BattleCruisers.Data.Models
{
    public interface ISkirmishModel
    {
        bool WasRandomPlayerCruiser { get; }
        HullKey PlayerCruiser { get; }
        bool WasRandomAICruiser { get; }
        HullKey AICruiser { get; }
        bool WasRandomStrategy { get; }
        StrategyType AIStrategy { get; }
        Difficulty Difficulty { get; }
        int BackgroundLevelNum { get; }
        string SkyMaterialName { get; }
    }
}