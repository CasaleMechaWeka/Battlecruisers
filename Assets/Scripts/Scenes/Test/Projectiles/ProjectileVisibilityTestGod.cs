using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class ProjectileVisibilityTestGod : MonoBehaviour
    {
        private ICameraAdjuster _cameraAdjuster;
        private Skybox _skybox;
        private ICircularList<Material> _skies;

        void Start()
        {
            Helper helper = new Helper();

            Artillery artillery = FindObjectOfType<Artillery>();
            helper.InitialiseBuilding(artillery, Faction.Blues, trackerFactory: CreateTrackerFactory());
            artillery.StartConstruction();

            AirFactory airFactory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(airFactory, Faction.Reds);
            airFactory.StartConstruction();

            _cameraAdjuster = CreateCameraAdjuster();

            _skies = FindSkyMaterials();

            _skybox = FindObjectOfType<Skybox>();
            Assert.IsNotNull(_skybox);
            ChangeSky();
        }

        private ITrackerFactory CreateTrackerFactory()
        {
            MarkerFactory markerFactory = GetComponent<MarkerFactory>();
            markerFactory.Intialise();

            return
                new TrackerFactory(
                    markerFactory,
                    new CameraBC(Camera.main));
        }

        // FELIX  Avoid duplicate code with NavigationTest scene, if it still exists :P
        private ICameraAdjuster CreateCameraAdjuster()
        {
            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            IBroadcastingFilter navigationWheelEnabledFilter = new StaticBroadcastingFilter(isMatch: true);
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel(navigationWheelEnabledFilter);

            Camera platformCamera = FindObjectOfType<Camera>();
            ICamera camera = new CameraBC(platformCamera);
            ICameraCalculatorSettings settings
                = new CameraCalculatorSettings(
                    Substitute.For<ISettingsManager>(),
                    camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(camera, settings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator = new CameraNavigationWheelCalculator(navigationWheelPanel, cameraCalculator, settings.ValidOrthographicSizes);
            ICameraTargetFinder cameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, camera);
            ICameraTargetProvider cameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cameraTargetFinder);

            // Smooth adjuster
            float smoothTime = 0.15f;

            return
                new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, smoothTime),
                    new SmoothPositionAdjuster(camera.Transform, smoothTime));
        }

        private ICircularList<Material> FindSkyMaterials()
        {
            IList<string> skyNames = new List<string>()
            {
                SkyMaterials.Blue,
                SkyMaterials.BlueCloudy,
                SkyMaterials.BlueDeep,
                SkyMaterials.Sunset,
                SkyMaterials.SunsetCloudy,
                SkyMaterials.SunsetWeirdClouds,
                SkyMaterials.White
            };

            IList<Material> skyMaterials = new List<Material>();
            IMaterialFetcher materialFetcher = new MaterialFetcher();

            foreach (string skyName in skyNames)
            {
                skyMaterials.Add(materialFetcher.GetMaterial(skyName));
            }

            return new CircularList<Material>(skyMaterials);
        }

        public void ChangeSky()
        {
            _skybox.material = _skies.Next();
        }

        void Update()
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}