using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class ShipVsGunshipBalancingTestGod : MultiCameraTestGod<ShipVsGunshipBalancingTest>
    {
        private IPrefabFactory _prefabFactory;

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcherLEGACY());
        }

        protected override void InitialiseScenario(ShipVsGunshipBalancingTest scenario)
        {
            scenario.Initialise(_prefabFactory, _updaterProvider);
        }
    }
}
