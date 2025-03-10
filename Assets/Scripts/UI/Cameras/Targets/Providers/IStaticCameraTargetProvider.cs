namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public interface IStaticCameraTargetProvider : IUserInputCameraTargetProvider
    {
        void SetTarget(ICameraTarget target);
    }
}