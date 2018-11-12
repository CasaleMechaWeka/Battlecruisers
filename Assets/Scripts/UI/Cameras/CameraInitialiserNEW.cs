using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
    public class CameraInitialiserNEW : MonoBehaviour
    {
        private ICameraAdjuster _cameraAdjuster;

        public float cameraSmoothTime;

        public ICamera SoleCamera { get; private set; }

        public void Initialise(ISettingsManager settingsManager)
        {
            Assert.IsNotNull(settingsManager);

            Camera platformCamera = GetComponent<Camera>();
            Assert.IsNotNull(platformCamera);
            SoleCamera = new CameraBC(platformCamera);

            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel();

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