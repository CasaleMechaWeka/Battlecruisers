namespace BattleCruisers.UI.Cameras
{
	public interface ICameraController : ICameraMover
    {
        void FocusOnPlayerCruiser();
        void FocusOnAiCruiser();
        void ShowFullMapView();
        void ShowMidLeft();
        void ShowMidRight();
    }
}
