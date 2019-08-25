namespace BattleCruisers.Scenes.Test.Turrets.Accuracy
{
    public class AccuracyTestGod : MultiCameraTestGod<AccuracyTest>
    {
        protected override void InitialiseScenario(AccuracyTest scenario)
        {
            scenario.Initialise(_updaterProvider);
        }

        protected override float OrderBy(AccuracyTest scenario)
        {
            return -scenario.gameObject.transform.position.y;
        }
    }
}