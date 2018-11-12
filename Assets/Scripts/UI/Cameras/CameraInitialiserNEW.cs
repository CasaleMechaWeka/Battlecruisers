using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
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

        // Can be called before Initialise(), due to wonderful dependencies :)
        private ICamera _camera;
        public ICamera SoleCamera
        {
            get
            {
                if (_camera == null)
                {
                    Camera platformCamera = GetComponent<Camera>();
                    Assert.IsNotNull(platformCamera);
                    _camera = new CameraBC(platformCamera);
                }

                return _camera;
            }
        }

        public void Initialise(INavigationWheelPanel navigationWheelPanel, ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(navigationWheelPanel, settingsManager);

            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, SoleCamera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(SoleCamera, settings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator = new CameraNavigationWheelCalculator(navigationWheelPanel, cameraCalculator, settings.ValidOrthographicSizes);
            ICameraTargetFinder cameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, SoleCamera);
            ICameraTargetProvider cameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cameraTargetFinder);

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