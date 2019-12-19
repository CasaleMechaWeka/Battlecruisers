namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public class StaticCameraTargetProvider : CameraTargetProvider, IStaticCameraTargetProvider
    {
        public void SetTarget(ICameraTarget target)
        {
            Target = target;
        }
    }
}