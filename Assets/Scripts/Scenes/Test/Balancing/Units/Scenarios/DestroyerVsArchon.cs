using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Units.Scenarios
{
    public class DestroyerVsArchon: ShipVsShipBalancingTest
    {
        protected override IPrefabKey LeftShipKey { get { return StaticPrefabKeys.Units.Destroyer; } }
        protected override IPrefabKey RightShipKey { get { return StaticPrefabKeys.Units.ArchonBattleship; } }
    }
}
