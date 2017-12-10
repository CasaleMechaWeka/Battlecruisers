using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Shields
{
    public class FrigateVsShields : ShipsVsShieldsBalancingTest
    {
        protected override IPrefabKey ShipKey { get { return StaticPrefabKeys.Units.Frigate; } }

        // Try to fit in 2 frigates
        protected override float ShipOffsetInM
        {
            get
            {
                float mortarRange = 19;
                float spaceForAFrigate = 5;
                return mortarRange - spaceForAFrigate - SHIELD_OFFSET_FROM_CENTRE_IN_M;
            }
        }
    }
}
