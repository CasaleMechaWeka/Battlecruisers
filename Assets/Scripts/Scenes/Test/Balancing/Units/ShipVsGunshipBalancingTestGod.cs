using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class ShipVsGunshipBalancingTestGod : MultiCameraTestGod<ShipVsGunshipBalancingTest>
    {
        private IPrefabFactory _prefabFactory;

        protected override async void InitialiseAsync(Helper parentHelper)
        {
            // FELIX  Cache Helper in parent class :)
            //_prefabFactory = await Helper.CreatePrefabFactoryAsync();
        }

        protected override void InitialiseScenario(ShipVsGunshipBalancingTest scenario)
        {
            scenario.Initialise(_prefabFactory, _updaterProvider);
        }
    }
}
