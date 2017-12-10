using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Shields
{
    public class DestroyerVsShields : ShipsVsShieldsBalancingTest
    {
        protected override IPrefabKey ShipKey { get { return StaticPrefabKeys.Units.Destroyer; } }

        // Try to fit in 2 destroyers
        protected override float ShipOffsetInM
        {
            get
            {
                float missileRange = 30;
                float spaceForADestroyer = 8;
                return missileRange - spaceForADestroyer - SHIELD_OFFSET_FROM_CENTRE_IN_M;
            }
        }
    }
}
