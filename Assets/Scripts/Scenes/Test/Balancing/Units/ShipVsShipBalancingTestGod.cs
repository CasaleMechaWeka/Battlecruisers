using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public abstract class ShipVsShipBalancingTestGod : MultiCameraTestGod<ShipVsShipBalancingTest>
    {
        private IPrefabFactory _prefabFactory;

        protected abstract IPrefabKey LeftShipKey { get; }
        protected abstract IPrefabKey RightShipKey { get; }

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcher());
        }

        protected override void InitialiseScenario(ShipVsShipBalancingTest scenario)
        {
            scenario.Initialise(_prefabFactory, LeftShipKey, RightShipKey);
        }
    }
}
