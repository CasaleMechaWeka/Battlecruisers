namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    public interface ICornerCameraTargetProvider
    {
        ICameraTarget GetTarget(CameraCorner cameraCorner);
    }
}