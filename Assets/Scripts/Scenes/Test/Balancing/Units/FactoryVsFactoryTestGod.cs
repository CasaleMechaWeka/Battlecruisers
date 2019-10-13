using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class FactoryVsFactoryTestGod : MultiCameraTestGod<FactoryVsFactoryTest>
    {
        private IPrefabFactory _prefabFactory;

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcherLEGACY());
        }

        protected override void InitialiseScenario(FactoryVsFactoryTest scenario)
        {
            scenario.Initialise(_prefabFactory, _updaterProvider);
        }
    }
}
