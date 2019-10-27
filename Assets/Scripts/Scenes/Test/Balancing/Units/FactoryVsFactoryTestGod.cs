namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class FactoryVsFactoryTestGod : MultiCameraTestGod<FactoryVsFactoryTest>
    {
        // FELIX  Seems to have same implementation as all sibling classes?  Move to parent?
        protected override void InitialiseScenario(FactoryVsFactoryTest scenario)
        {
            scenario.Initialise(_baseHelper);
        }
    }
}
