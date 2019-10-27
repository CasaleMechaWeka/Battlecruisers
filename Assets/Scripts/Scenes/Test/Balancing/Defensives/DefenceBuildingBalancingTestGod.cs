using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public abstract class DefenceBuildingBalancingTestGod : MultiCameraTestGod<DefenceBuildingBalancingTest>
    {
        private IPrefabFactory _prefabFactory;

        protected abstract IPrefabKey UnitKey { get; }
        protected abstract IPrefabKey BasicDefenceBuildingKey { get; }
        protected abstract IPrefabKey AdvancedDefenceBuildingKey { get; }

        // FELIX  Try test scene :)  Will break, InitialiseScenario will have null _prefabFactory :(
        protected async override void InitialiseAsync(Helper parentHelper)
        // FELIX  Cache Helper in parent class :)
        {
            //_prefabFactory = await Helper.CreatePrefabFactoryAsync();
        }

        protected override void InitialiseScenario(DefenceBuildingBalancingTest scenario)
        {
            scenario.Initialise(_prefabFactory, UnitKey, BasicDefenceBuildingKey, AdvancedDefenceBuildingKey, _updaterProvider);
        }
    }
}
