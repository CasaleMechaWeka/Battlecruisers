using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Shields
{
    public class ArchonVsShields : ShipsVsShieldsBalancingTest
    {
        protected override IPrefabKey ShipKey { get { return StaticPrefabKeys.Units.ArchonBattleship; } }
    }
}
