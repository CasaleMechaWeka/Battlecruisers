using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras
{
	public class CameraController : CameraMover, ICameraController
	{
        private ICameraTransitionManager _transitionManager;
		private ICameraMover _userInputMover;

		private ICameraMover _currentMover;
		private ICameraMover CurrentMover
		{
			get { return _currentMover; }
			set
			{
				if (_currentMover != null)
				{
					_currentMover.StateChanged -= _currentMover_StateChanged;
				}

                _currentMover = value;

				if (_currentMover != null)
				{
                    _currentMover.Reset(State);
					_currentMover.StateChanged += _currentMover_StateChanged;
				}
			}
		}

        // Not constructor because of circular dependencies with cruisers
		public void Initialise(ICameraTransitionManager cameraTransitionManager, ICameraMover userInputMover)
		{
			Helper.AssertIsNotNull(cameraTransitionManager, userInputMover);

			_transitionManager = cameraTransitionManager;
			_userInputMover = userInputMover;

			CurrentMover = _userInputMover;
		}

		public override void MoveCamera(float deltaTime)
		{
			CurrentMover.MoveCamera(deltaTime);
		}

        public void FocusOnPlayerCruiser()
		{
			HandleNavigationButtonPress(CameraState.PlayerCruiser);
		}

		public void FocusOnAiCruiser()
		{
			HandleNavigationButtonPress(CameraState.AiCruiser);
		}

		public void ShowFullMapView()
		{
			HandleNavigationButtonPress(CameraState.Overview);
		}

		public void ShowMidLeft()
		{
			HandleNavigationButtonPress(CameraState.LeftMid);
		}

		public void ShowMidRight()
		{
			HandleNavigationButtonPress(CameraState.RightMid);
		}

		private void HandleNavigationButtonPress(CameraState newState)
		{
		    if (newState != State)
			{
                CurrentMover = _transitionManager;
                _transitionManager.CameraTarget = newState;
			}
		}

		private void _currentMover_StateChanged(object sender, CameraStateChangedArgs e)
		{
			State = e.NewState;

			if (e.PreviousState == CameraState.InTransition)
			{
				// Transition complete, revert to default camera mover
				CurrentMover = _userInputMover;
			}
		}
	}
}
