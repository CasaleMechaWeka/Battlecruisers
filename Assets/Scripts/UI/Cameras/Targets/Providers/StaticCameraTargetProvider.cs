namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    /// <summary>
    /// FELIX  Not really a user input camera target provider :/
    /// </summary>
    public class StaticCameraTargetProvider : UserInputCameraTargetProvider, IStaticCameraTargetProvider
    {
        public override int Priority { get; }

        public StaticCameraTargetProvider(int priority)
        {
            Priority = priority;
        }

        public void SetTarget(ICameraTarget target)
        {
            Target = target;
        }
    }
}