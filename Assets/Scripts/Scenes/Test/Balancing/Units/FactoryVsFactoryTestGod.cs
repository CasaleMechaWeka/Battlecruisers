using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    // FELIX  Try test scene :)
    public class FactoryVsFactoryTestGod : MultiCameraTestGod<FactoryVsFactoryTest>
    {
        private IPrefabFactory _prefabFactory;

        protected async override void InitialiseAsync()
        {
            _prefabFactory = await Helper.CreatePrefabFactoryAsync();
        }

        protected override void InitialiseScenario(FactoryVsFactoryTest scenario)
        {
            scenario.Initialise(_prefabFactory, _updaterProvider);
        }
    }
}
