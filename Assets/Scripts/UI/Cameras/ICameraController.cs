using System;

namespace BattleCruisers.UI.Cameras
{
	public interface ICameraController
    {
        CameraState State { get; }

		event EventHandler<CameraStateChangedArgs> StateChanged;

        void FocusOnPlayerCruiser();
        void FocusOnAiCruiser();
        void ShowFullMapView();
        void ShowMidLeft();
        void ShowMidRight();
    }
}
