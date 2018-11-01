namespace BattleCruisers.UI.Cameras
{
    // FELIX  Similar to ICameraMover.  Remove ICameraMover?
    public interface ICameraAdjuster
    {
        void AdjustCamera(float deltaTime);
    }
}