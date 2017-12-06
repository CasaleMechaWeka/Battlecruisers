using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class ArchonVsGunships : ShipVsGunshipBalancingTest
    {
        protected override IPrefabKey ShipKey { get { return StaticPrefabKeys.Units.ArchonBattleship; } }
    }
}
