using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public abstract class AntiSeaBalancingTestGod : DefenceBuildingBalancingTestGod
    {
        protected override IPrefabKey BasicDefenceBuildingKey { get { return StaticPrefabKeys.Buildings.AntiShipTurret; } }
        protected override IPrefabKey AdvancedDefenceBuildingKey { get { return StaticPrefabKeys.Buildings.Mortar; } }
   }
}
