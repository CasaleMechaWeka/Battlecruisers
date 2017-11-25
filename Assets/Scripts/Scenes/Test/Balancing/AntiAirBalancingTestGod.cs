using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class AntiAirBalancingTestGod : DefenceBuildingBalancingTestGod
    {
        protected override IPrefabKey UnitKey { get { return StaticPrefabKeys.Units.Bomber; } }
        protected override IPrefabKey BasicDefenceBuildingKey { get { return StaticPrefabKeys.Buildings.AntiAirTurret; } }
        protected override IPrefabKey AdvancedDefenceBuildingKey { get { return StaticPrefabKeys.Buildings.SamSite; }}
   }
}