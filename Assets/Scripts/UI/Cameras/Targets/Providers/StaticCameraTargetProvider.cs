namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    /// <summary>
    /// FELIX  Not really a user input camera target provider :/
    /// </summary>
    public class StaticCameraTargetProvider : UserInputCameraTargetProvider, IStaticCameraTargetProvider
    {
        public override int Priority => 6;

        public void SetTarget(ICameraTarget target)
        {
            Target = target;
        }
    }
}