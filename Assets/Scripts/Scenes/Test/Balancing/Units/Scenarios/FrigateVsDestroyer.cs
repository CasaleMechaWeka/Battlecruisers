using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class FrigateVsDestroyer : ShipVsShipBalancingTest
    {
        protected override IPrefabKey LeftShipKey { get { return StaticPrefabKeys.Units.Frigate; } }
        protected override IPrefabKey RightShipKey { get { return StaticPrefabKeys.Units.Destroyer; } }
    }
}
