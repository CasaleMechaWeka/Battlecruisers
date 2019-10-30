namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class FactoryVsFactoryTestGod : MultiCameraTestGod<FactoryVsFactoryTest>
    {
        protected override void InitialiseScenario(FactoryVsFactoryTest scenario)
        {
            scenario.Initialise(_baseHelper);
        }
    }
}
