using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Shields
{
    public class AttackBoatVsShields : ShipsVsShieldsBalancingTest
    {
        protected override IPrefabKey ShipKey { get { return StaticPrefabKeys.Units.AttackBoat; } }
    }
}
