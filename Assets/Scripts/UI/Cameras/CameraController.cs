using System;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
	// FELIX  Make this class testable :)
	public class CameraController : MonoBehaviour, ICameraController
	{
		// FELIX  Remove unused fields :)
		private Camera _camera;
        private ICameraCalculator _cameraCalculator;
        private ISettingsManager _settingsManager;
        private IFilter _shouldNavigationBeEnabledFilter;
		private ICameraTransitionManager _transitionManager;
		private ICameraMover _userInputMover;

		// User input
		private IScrollHandler _scrollHandler;
		private IMouseZoomHandler _mouseZoomHandler;

		public float smoothTime;

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
					_currentMover.StateChanged += _currentMover_StateChanged;
				}
			}
		}

		private CameraState _state;
        public CameraState State
        {
            get { return _state; }
            private set
            {
                if (StateChanged != null)
                {
                    StateChanged.Invoke(this, new CameraStateChangedArgs(_state, value));
                }

                _state = value;
            }
        }

		public event EventHandler<CameraStateChangedArgs> StateChanged;
        
		// Dragging
		private const float CAMERA_POSITION_MAX_X = 35;
		private const float CAMERA_POSITION_MIN_X = -35;
		private const float CAMERA_POSITION_MAX_Y = 30;
		private const float CAMERA_POSITION_MIN_Y = 0;

        // FELIX  Inject everything, from new CameraInitialise perhaps?
		public void Initialise(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            ISettingsManager settingsManager, 
            Material skyboxMaterial,
            IFilter shouldNavigationBeEnabledFilter)
		{
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, settingsManager, skyboxMaterial, shouldNavigationBeEnabledFilter);

            _settingsManager = settingsManager;
            _shouldNavigationBeEnabledFilter = shouldNavigationBeEnabledFilter;

            _camera = GetComponent<Camera>();
            Assert.IsNotNull(_camera);
			ICamera camera = new CameraBC(_camera);

            Skybox skybox = GetComponent<Skybox>();
			Assert.IsNotNull(skybox);
			skybox.material = skyboxMaterial;

            _cameraCalculator = new CameraCalculator(_camera);
			_state = CameraState.Overview;

			// FELIX  Inject
			// Handle transitions (triggered by navigation buttons)
			ICameraTargetsFactory cameraTargetsFactory
				= new CameraTargetsFactory(
				    camera,
    				_cameraCalculator,
    				playerCruiser,
    				aiCruiser);

			_transitionManager
				= new CameraTransitionManager(
					camera,
				    cameraTargetsFactory,
    				new SmoothPositionAdjuster(_camera.transform, smoothTime),
    				new SmoothZoomAdjuster(_camera, smoothTime));

			// FELIX  Move to factory and inject factory :)
			// Handle user input (scrolling by screen edge, zooming via mouse wheel)
			IScreen screen = new ScreenBC();
			Rectangle cameraBounds = new Rectangle(CAMERA_POSITION_MIN_X, CAMERA_POSITION_MAX_X, CAMERA_POSITION_MIN_Y, CAMERA_POSITION_MAX_Y);
			IPositionClamper cameraPositionClamper = new PositionClamper(cameraBounds);
			_scrollHandler = new ScrollHandler(_cameraCalculator, screen, cameraPositionClamper);

			_mouseZoomHandler = new MouseZoomHandler(_settingsManager, CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE, CameraCalculator.MAX_CAMERA_ORTHOGRAPHIC_SIZE);

			_userInputMover = new UserInputCameraMover(camera, new InputBC(), _scrollHandler, _mouseZoomHandler);
			CurrentMover = _userInputMover;

			FocusOnPlayerCruiser();
		}

		void Update()
		{
			CurrentMover.MoveCamera(Time.deltaTime, State);
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
			bool willCameraMove = _transitionManager.SetCameraTarget(newState);
			if (willCameraMove)
			{
				CurrentMover = _transitionManager;
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
