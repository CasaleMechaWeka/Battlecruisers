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

    public interface ICameraMover
    {
        CameraState State { get; }

        event EventHandler<CameraStateChangedArgs> StateChanged;

		/// <summary>
		/// Should be called every frame to update the camera position/zoom.
		/// </summary>
		void MoveCamera(float deltaTime, CameraState currentState);
	}
}
