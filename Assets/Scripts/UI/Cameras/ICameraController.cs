using System;

namespace BattleCruisers.UI.Cameras
{
	// FELIX  Move to ICameraMover?
    public enum CameraState
    {
		// FELIX  Rename PlayerInputControlled to UserInputController :P
        PlayerCruiser, AiCruiser, Overview, InTransition, LeftMid, RightMid, PlayerInputControlled
    }

	// FELIX  Remove
    public class CameraTransitionArgs : EventArgs
    {
        public CameraState Origin { get; private set; }
        public CameraState Destination { get; private set; }

        public CameraTransitionArgs(CameraState origin, CameraState destination)
        {
            Origin = origin;
            Destination = destination;
        }
    }

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
