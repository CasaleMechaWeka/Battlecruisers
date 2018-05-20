using System;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras
{
	public abstract class CameraMover : ICameraMover
    {
        private CameraState _state;
        public CameraState State 
        { 
            get { return _state; }
			protected set
            {
                // Event handlers may access this property, so want to update the 
                // value before emitting the changed event.
                CameraState oldState = _state;
                _state = value;

                if (oldState != _state && StateChanged != null)
                {
                    Logging.Log(Tags.CAMERA, "CameraMover.State: " + oldState + " > " + value);
                    StateChanged.Invoke(this, new CameraStateChangedArgs(oldState, _state));
                }
            }
        }

        public event EventHandler<CameraStateChangedArgs> StateChanged;

		protected CameraMover()
        {
            _state = CameraState.UserInputControlled;
        }

		public abstract void MoveCamera(float deltaTime);

        public void Reset(CameraState currentState)
        {
            Logging.Log(Tags.CAMERA, "CameraMover.Reset(): " + currentState);
            _state = currentState;
        }
	}
}
