using System;

namespace BattleCruisers.UI.Cameras
{
	// FELIX  Move to ICameraTransitionManager?
    public enum CameraState
    {
		// FELIX  Remove PlayerInputControlled?
        PlayerCruiser, AiCruiser, Overview, InTransition, LeftMid, RightMid, PlayerInputControlled
    }

	// FELIX  Move to ICameraTransitionManager?
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

		event EventHandler<CameraTransitionArgs> CameraTransitionStarted;
		event EventHandler<CameraTransitionArgs> CameraTransitionCompleted;

        void FocusOnPlayerCruiser();
        void FocusOnAiCruiser();
        void ShowFullMapView();
        void ShowMidLeft();
        void ShowMidRight();
    }
}
