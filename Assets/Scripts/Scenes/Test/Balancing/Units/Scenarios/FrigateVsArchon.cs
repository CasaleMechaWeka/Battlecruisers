using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

// FELIX  Update namespace to reflect folder structure
namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class FrigateVsArchon: ShipVsShipBalancingTest
    {
        protected override IPrefabKey LeftShipKey { get { return StaticPrefabKeys.Units.Frigate; } }
        protected override IPrefabKey RightShipKey { get { return StaticPrefabKeys.Units.ArchonBattleship; } }
    }
}
