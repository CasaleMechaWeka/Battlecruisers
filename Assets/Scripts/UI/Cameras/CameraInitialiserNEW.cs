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

namespace BattleCruisers.UI.Cameras
{
    public class CameraInitialiserNEW : MonoBehaviour
    {
        public ICameraAdjuster CameraAdjuster { get; private set; }

        public float cameraSmoothTime;

        public ICameraFocuser  Initialise(
            ICamera camera, 
            ISettingsManager settingsManager, 
            ICruiser playerCruiser, 
            ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(camera, settingsManager, playerCruiser, aiCruiser);

            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel();

            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(camera, settings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator = new CameraNavigationWheelCalculator(navigationWheelPanel, cameraCalculator, settings.ValidOrthographicSizes);
            ICameraTargetFinder coreCameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, camera);
            ICameraTargetFinder cornerCameraTargetFinder
                = new NavigationWheelCornersCameraTargetFinder(
                    coreCameraTargetFinder,
                    new CornerIdentifier(),
                    new CornerCameraTargetProvider(camera, cameraCalculator, playerCruiser, aiCruiser));
            ICameraTargetProvider cameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cornerCameraTargetFinder);

            CameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, cameraSmoothTime),
                    new SmoothPositionAdjuster(camera.Transform, cameraSmoothTime));

            return new CameraFocuser(navigationWheelPanel.PanelArea, navigationWheelPanel.NavigationWheel);
        }

        public void Update()
        {
            CameraAdjuster.AdjustCamera();
        }
    }
}