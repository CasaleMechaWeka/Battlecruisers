using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public class FrigateBalancingTestGod : AntiSeaBalancingTestGod
    {
        protected override IPrefabKey UnitKey => StaticPrefabKeys.Units.Frigate;
    }
}
