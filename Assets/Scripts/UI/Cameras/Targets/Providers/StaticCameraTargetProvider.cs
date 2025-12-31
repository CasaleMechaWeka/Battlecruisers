namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public class StaticCameraTargetProvider : UserInputCameraTargetProvider
    {
        public override int Priority { get; }

        public StaticCameraTargetProvider(int priority)
        {
            Priority = priority;
        }

        public void SetTarget(CameraTarget target)
        {
            Target = target;
        }
    }
}