using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class ProjectileVisibilityTestGod : NavigationTestGod
    {
        private Skybox _skybox;
        private ICircularList<Material> _skies;

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper();

            Artillery artillery = FindObjectOfType<Artillery>();
            helper.InitialiseBuilding(artillery, Faction.Blues, trackerFactory: CreateTrackerFactory());
            artillery.StartConstruction();

            AirFactory airFactory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(airFactory, Faction.Reds);
            airFactory.StartConstruction();

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
    }
}