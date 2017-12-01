using BattleCruisers.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class ShipVsShipBalancingTestGod : MultiCameraTestGod<ShipVsShipBalancingTest>
    {
        private IPrefabFactory _prefabFactory;

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcher());
        }

        protected override void InitialiseScenario(ShipVsShipBalancingTest scenario)
        {
            scenario.Initialise(_prefabFactory);
        }
    }
}
