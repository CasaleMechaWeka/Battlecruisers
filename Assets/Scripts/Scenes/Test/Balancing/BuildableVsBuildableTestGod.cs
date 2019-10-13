using BattleCruisers.Utils.Fetchers;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildableVsBuildableTestGod : MultiCameraTestGod<BuildableVsBuildableTest>
    {
        private IPrefabFactory _prefabFactory;
        protected TestUtils.Helper _helper;

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcherLEGACY());
            _helper = new TestUtils.Helper(updaterProvider: _updaterProvider);
        }

        protected override void InitialiseScenario(BuildableVsBuildableTest scenario)
        {
            scenario.Initialise(_prefabFactory, _helper, _updaterProvider);
        }
    }
}
