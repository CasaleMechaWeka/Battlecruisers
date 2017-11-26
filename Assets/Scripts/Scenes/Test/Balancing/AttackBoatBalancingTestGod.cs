using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class AttackBoatBalancingTestGod : AntiSeaBalancingTestGod
    {
        protected override IPrefabKey UnitKey { get { return StaticPrefabKeys.Units.AttackBoat; } }
    }
}
