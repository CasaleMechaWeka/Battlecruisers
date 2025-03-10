using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public abstract class AntiSeaBalancingTestGod : DefenceBuildingBalancingTestGod
    {
        protected override IPrefabKey BasicDefenceBuildingKey => StaticPrefabKeys.Buildings.AntiShipTurret;
        protected override IPrefabKey AdvancedDefenceBuildingKey => StaticPrefabKeys.Buildings.Mortar;
   }
}
