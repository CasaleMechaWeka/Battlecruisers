using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
	public class CameraInitialiser : MonoBehaviour
	{
		public float smoothTime;

		private CameraController _cameraController;
		public ICameraController CameraController { get { return _cameraController; } }

		public IUserInputCameraMover UserInputCameraMover { get; private set; }
        public ICamera MainCamera { get; private set; }

		// Scrolling bounds
		private const float CAMERA_POSITION_MAX_X = 45;
		private const float CAMERA_POSITION_MIN_X = -45;
		private const float CAMERA_POSITION_MAX_Y = 30;
		private const float CAMERA_POSITION_MIN_Y = 0;

        // Two initialise methods because of circular dependency between cruisers
        // and the camera controller.
        public void StaticInitialise()
		{
			_cameraController = new CameraController();

			Camera platformCamera = GetComponent<Camera>();
			Assert.IsNotNull(platformCamera);
			MainCamera = new CameraBC(platformCamera);
		}

		public void Initialise(
			ICruiser playerCruiser,
			ICruiser aiCruiser,
			ISettingsManager settingsManager,
			Material skyboxMaterial,
			INavigationSettings navigationSettings,
            IPauseGameManager pauseGameManager)
		{
			Helper.AssertIsNotNull(playerCruiser, aiCruiser, settingsManager, skyboxMaterial, navigationSettings, pauseGameManager);

			Skybox skybox = GetComponent<Skybox>();
			Assert.IsNotNull(skybox);
			skybox.material = skyboxMaterial;

            ICameraCalculator cameraCalculator = new CameraCalculator(MainCamera, settingsManager);
			ICameraTransitionManager transitionManager = CreateTransitionManager(playerCruiser, aiCruiser, MainCamera, cameraCalculator, navigationSettings);
			UserInputCameraMover = CreateUserInputMover(settingsManager, MainCamera, cameraCalculator, navigationSettings);

            ICameraMover dummyMover = new DummyCameraMover();

			_cameraController.Initialise(pauseGameManager, transitionManager, UserInputCameraMover, dummyMover);
		}

		private IUserInputCameraMover CreateUserInputMover(
			ISettingsManager settingsManager, 
			ICamera camera, 
			ICameraCalculator cameraCalculator, 
			INavigationSettings navigationSettings)
		{
			IScreen screen = new ScreenBC();
			Rectangle cameraBounds = new Rectangle(CAMERA_POSITION_MIN_X, CAMERA_POSITION_MAX_X, CAMERA_POSITION_MIN_Y, CAMERA_POSITION_MAX_Y);
			IPositionClamper cameraPositionClamper = new PositionClamper(cameraBounds);
            IDeltaTimeProvider deltaTimeProvider = new TimeBC();
            IScrollPositionFinder scrollPositionFinder = new ScreenEdgePositionFinder(screen);

            IScrollHandler scrollHandler
				= new ScrollHandler(
					cameraCalculator,
					cameraPositionClamper,
                    deltaTimeProvider,
                    scrollPositionFinder);

			IMouseZoomHandler mouseZoomHandler
				= new MouseZoomHandler(
					settingsManager,
                    deltaTimeProvider,
					CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE,
					CameraCalculator.MAX_CAMERA_ORTHOGRAPHIC_SIZE);

            return
				new UserInputCameraMover(
					camera,
					new InputBC(),
					scrollHandler,
					mouseZoomHandler,
					navigationSettings);
		}

		private ICameraTransitionManager CreateTransitionManager(
			ICruiser playerCruiser, 
			ICruiser aiCruiser, 
			ICamera camera, 
			ICameraCalculator cameraCalculator,
			INavigationSettings navigationSettings)
		{
			ICameraTargetsFactory cameraTargetsFactory
    			= new CameraTargetsFactory(
    				camera,
    				cameraCalculator,
    				playerCruiser,
    				aiCruiser);

        	return
			    new CameraTransitionManager(
        			camera,
        			cameraTargetsFactory,
        			new SmoothPositionAdjuster(camera, smoothTime),
					new SmoothZoomAdjuster(camera, smoothTime),
				    navigationSettings);
		}
	}
}
