namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildableVsBuildableTestGod : MultiCameraTestGod<BuildableVsBuildableTest>
    {
        protected override void InitialiseScenario(BuildableVsBuildableTest scenario)
        {
            scenario.Initialise(_baseHelper);
        }
    }
}
