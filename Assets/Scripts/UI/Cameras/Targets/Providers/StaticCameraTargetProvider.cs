namespace BattleCruisers.UI.Cameras.Targets.Providers
{
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