using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
    public class CameraInitialiserNEW : MonoBehaviour
    {
        private ICameraAdjuster _cameraAdjuster;

        public float cameraSmoothTime;

        // FELIX  Inject, to avoid stupid lazy instantiation :P
        // Not in Initialise() method because of circular dependency with cruisers.
        private ICamera _soleCamera;
        public ICamera SoleCamera
        {
            get
            {
                if (_soleCamera == null)
                {
                    Camera platformCamera = GetComponent<Camera>();
                    Assert.IsNotNull(platformCamera);
                    _soleCamera = new CameraBC(platformCamera);
                }

                return _soleCamera;
            }
        }

        public void Initialise(ISettingsManager settingsManager, ICruiser playerCruiser, ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(settingsManager, playerCruiser, aiCruiser);

            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel();

            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, SoleCamera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(SoleCamera, settings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator = new CameraNavigationWheelCalculator(navigationWheelPanel, cameraCalculator, settings.ValidOrthographicSizes);
            ICameraTargetFinder coreCameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, SoleCamera);
            ICameraTargetFinder cornerCameraTargetFinder
                = new NavigationWheelCornersCameraTargetFinder(
                    coreCameraTargetFinder,
                    new CornerIdentifier(),
                    new CornerCameraTargetProvider(SoleCamera, cameraCalculator, playerCruiser, aiCruiser));
            ICameraTargetProvider cameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cornerCameraTargetFinder);

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(SoleCamera, cameraSmoothTime),
                    new SmoothPositionAdjuster(SoleCamera.Transform, cameraSmoothTime));
        }

        public void Update()
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}