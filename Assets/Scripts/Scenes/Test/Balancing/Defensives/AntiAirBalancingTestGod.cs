using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public class AntiAirBalancingTestGod : DefenceBuildingBalancingTestGod
    {
        protected override IPrefabKey UnitKey => StaticPrefabKeys.Units.Bomber;
        protected override IPrefabKey BasicDefenceBuildingKey => StaticPrefabKeys.Buildings.AntiAirTurret;
        protected override IPrefabKey AdvancedDefenceBuildingKey => StaticPrefabKeys.Buildings.SamSite;
   }
}
