using System;

namespace BattleCruisers.UI.Cameras
{
	public class CameraStateChangedArgs : EventArgs
    {
		public CameraState PreviousState { get; private set; }
		public CameraState NewState { get; private set; }

		public CameraStateChangedArgs(CameraState previousState, CameraState newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }
    }

    // FELIX  Create base class for both IMovers and CameraController (wait until
	// CameraController does not extend MonoBehaviour first :P)
    public interface ICameraMover
    {
        CameraState State { get; }

        event EventHandler<CameraStateChangedArgs> StateChanged;

		/// <summary>
		/// Should be called every frame to update the camera position/zoom.
		/// </summary>
		void MoveCamera(float deltaTime, CameraState currentState);
		void Reset();
	}
}
