using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
	public class CameraController : CameraMover, ICameraController, IManagedDisposable
	{
        private IPauseGameManager _pauseGameManager;
        private ICameraTransitionManager _transitionManager;
        private ICameraMover _userInputMover, _dummyMover, _prePauseMover;

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
            IPauseGameManager pauseGameManager, 
            ICameraTransitionManager cameraTransitionManager, 
            ICameraMover userInputMover,
            ICameraMover dummyMover)
		{
			Helper.AssertIsNotNull(pauseGameManager, cameraTransitionManager, userInputMover, dummyMover);

            _pauseGameManager = pauseGameManager;
			_transitionManager = cameraTransitionManager;
			_userInputMover = userInputMover;
            _dummyMover = dummyMover;

			CurrentMover = _userInputMover;

            _pauseGameManager.GamePaused += _pauseGameManager_GamePaused;
            _pauseGameManager.GameResumed += _pauseGameManager_GameResumed;
		}

        private void _pauseGameManager_GamePaused(object sender, EventArgs e)
        {
            _prePauseMover = CurrentMover;
            CurrentMover = _dummyMover;
        }

        private void _pauseGameManager_GameResumed(object sender, EventArgs e)
        {
            Assert.IsNotNull(_prePauseMover, "Received resume game without preceding pause game.");

            CurrentMover = _prePauseMover;
            _prePauseMover = null;
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

        public void DisposeManagedState()
        {
            _pauseGameManager.GamePaused -= _pauseGameManager_GamePaused;
            _pauseGameManager.GameResumed -= _pauseGameManager_GameResumed;
        }
    }
}
