using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
	// FELIX  Test :P
	public class CameraController : CameraMover, ICameraController
	{
        private ICameraTransitionManager _transitionManager;
		private ICameraMover _userInputMover;
		private IFilter _shouldNavigationBeEnabledFilter;

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
		public void Initialise(
			ICameraTransitionManager cameraTransitionManager,
			ICameraMover userInputMover,
            IFilter shouldNavigationBeEnabledFilter)
		{
			Helper.AssertIsNotNull(cameraTransitionManager, userInputMover, shouldNavigationBeEnabledFilter);

			_transitionManager = cameraTransitionManager;
			_userInputMover = userInputMover;
            _shouldNavigationBeEnabledFilter = shouldNavigationBeEnabledFilter;

			CurrentMover = _userInputMover;

			FocusOnPlayerCruiser();
		}

		public override void MoveCamera(float deltaTime)
		{
			// FELIX
			//if (_shouldNavigationBeEnabledFilter.IsMatch)
			{
                CurrentMover.MoveCamera(Time.deltaTime);
			}
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
			// FELIX
			//if (_shouldNavigationBeEnabledFilter.IsMatch 
			    //&& newState != State)
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
