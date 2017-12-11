using BattleCruisers.Fetchers;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildableVsBuildableTestGod : MultiCameraTestGod<BuildableVsBuildableTest>
    {
        private IPrefabFactory _prefabFactory;
        private TestUtils.Helper _helper;

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcher());
            _helper = new TestUtils.Helper();
        }

        protected override void InitialiseScenario(BuildableVsBuildableTest scenario)
        {
            scenario.Initialise(_prefabFactory, _helper);
        }
    }
}
