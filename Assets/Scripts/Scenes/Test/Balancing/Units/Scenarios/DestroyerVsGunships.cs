using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Units.Scenarios
{
    public class DestroyerVsGunships : ShipVsGunshipBalancingTest
    {
        protected override IPrefabKey ShipKey { get { return StaticPrefabKeys.Units.Destroyer; } }
    }
}
