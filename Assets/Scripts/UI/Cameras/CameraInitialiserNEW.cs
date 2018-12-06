using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Filters;
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

        public ICameraComponents Initialise(
            ICamera camera, 
            ISettingsManager settingsManager, 
            ICruiser playerCruiser, 
            ICruiser aiCruiser,
            IBroadcastingFilter navigationWheelEnabledFilter,
            Material skyboxMaterial)
        {
            Helper.AssertIsNotNull(camera, settingsManager, playerCruiser, aiCruiser, navigationWheelEnabledFilter, skyboxMaterial);

            SetupSkybox(skyboxMaterial);

            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel(navigationWheelEnabledFilter);

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

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, cameraSmoothTime),
                    new SmoothPositionAdjuster(camera.Transform, cameraSmoothTime));

            return
                new CameraComponents(
                    _cameraAdjuster,
                    navigationWheelPanel.NavigationWheel,
                    new CameraFocuser(navigationWheelPanel.PanelArea, navigationWheelPanel.NavigationWheel));
        }

        private void SetupSkybox(Material skyboxMaterial)
        {
            Skybox skybox = GetComponent<Skybox>();
            Assert.IsNotNull(skybox);
            skybox.material = skyboxMaterial;
        }

        public void Update()
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}