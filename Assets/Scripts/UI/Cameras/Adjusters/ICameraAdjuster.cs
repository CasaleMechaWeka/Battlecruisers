namespace BattleCruisers.UI.Cameras.Adjusters
{
    // FELIX  Similar to ICameraMover.  Remove ICameraMover?
    public interface ICameraAdjuster
    {
        // FELIX  Delta time is unused???
        void AdjustCamera(float deltaTime);
    }
}