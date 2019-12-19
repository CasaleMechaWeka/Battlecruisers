namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public interface IStaticCameraTargetProvider : ICameraTargetProvider
    {
        void SetTarget(ICameraTarget target);
    }
}