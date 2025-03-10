using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public abstract class DefenceBuildingBalancingTestGod : MultiCameraTestGod<DefenceBuildingBalancingTest>
    {
        protected abstract IPrefabKey UnitKey { get; }
        protected abstract IPrefabKey BasicDefenceBuildingKey { get; }
        protected abstract IPrefabKey AdvancedDefenceBuildingKey { get; }

        protected override void InitialiseScenario(DefenceBuildingBalancingTest scenario)
        {
            scenario.Initialise(_baseHelper, UnitKey, BasicDefenceBuildingKey, AdvancedDefenceBuildingKey);
        }
    }
}
