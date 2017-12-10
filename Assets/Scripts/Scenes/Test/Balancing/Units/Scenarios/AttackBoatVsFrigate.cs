using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Units.Scenarios
{
    public class AttackBoatVsFrigate : ShipVsShipBalancingTest
    {
        protected override IPrefabKey LeftShipKey { get { return StaticPrefabKeys.Units.AttackBoat; } }
        protected override IPrefabKey RightShipKey { get { return StaticPrefabKeys.Units.Frigate; } }
    }
}
