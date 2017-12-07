using BattleCruisers.Fetchers;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Shields
{
    public class ShipsVsShieldsBalancingTestGod : MultiCameraTestGod<ShipsVsShieldsBalancingTest>
    {
        private IPrefabFactory _prefabFactory;
        private TestUtils.Helper _helper;

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcher());
            _helper = new TestUtils.Helper();
        }

        protected override void InitialiseScenario(ShipsVsShieldsBalancingTest scenario)
        {
            scenario.Initialise(_prefabFactory, _helper);
        }
    }
}
