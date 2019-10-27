using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    // FELIX  Try test scene :)
    public class FactoryVsFactoryTestGod : MultiCameraTestGod<FactoryVsFactoryTest>
    {
        private Helper _parentHelper;

        protected override void InitialiseAsync(Helper helper)
        {
            base.InitialiseAsync(helper);
            _parentHelper = helper;
        }

        protected override void InitialiseScenario(FactoryVsFactoryTest scenario)
        {
            scenario.Initialise(_parentHelper);
        }
    }
}
