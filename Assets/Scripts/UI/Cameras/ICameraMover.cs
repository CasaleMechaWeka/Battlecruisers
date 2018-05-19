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

		void MoveCamera(CameraState currentState);
	}
}
