using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class CloudTestGod : NavigationTestGod
    {
        private Skybox _skybox;
        private ICircularList<Material> _skies;

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper();

            _skies = FindSkyMaterials();

            _skybox = FindObjectOfType<Skybox>();
            Assert.IsNotNull(_skybox);
            ChangeSky();

            CloudInitialiser cloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(new StaticData().Levels[7], new BCUtils.RandomGenerator());
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