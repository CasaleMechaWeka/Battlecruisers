using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildableVsBuildableTestGod : MultiCameraTestGod<BuildableVsBuildableTest>
    {
        private IPrefabFactory _prefabFactory;
        protected Helper _helper;

        protected async override void InitialiseAsync()
        {
            _prefabFactory = await Helper.CreatePrefabFactoryAsync();
            _helper = new Helper(updaterProvider: _updaterProvider);
        }

        protected override void InitialiseScenario(BuildableVsBuildableTest scenario)
        {
            scenario.Initialise(_prefabFactory, _helper, _updaterProvider);
        }
    }
}
