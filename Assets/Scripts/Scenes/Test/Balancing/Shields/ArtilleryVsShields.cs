using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Shields
{
    public class ArtilleryVsShields : OffensivesVsShieldsBalancingTest
    {
        protected override IPrefabKey OffensiveKey { get { return StaticPrefabKeys.Buildings.Artillery; } }
    }
}
