using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public abstract class DefenceBuildingBalancingTestGod : MultiCameraTestGod<DefenceBuildingBalancingTest>
    {
        private IPrefabFactory _prefabFactory;

        protected abstract IPrefabKey UnitKey { get; }
        protected abstract IPrefabKey BasicDefenceBuildingKey { get; }
        protected abstract IPrefabKey AdvancedDefenceBuildingKey { get; }

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcherLEGACY());
        }

        protected override void InitialiseScenario(DefenceBuildingBalancingTest scenario)
        {
            scenario.Initialise(_prefabFactory, UnitKey, BasicDefenceBuildingKey, AdvancedDefenceBuildingKey, _updaterProvider);
        }
    }
}
