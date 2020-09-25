using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class CloudV2TestGod : NavigationTestGod
    {
        public SkyStatsGroup skyStatsGroup;
        public SkyButtonGroup skyButtonGroup;
        public LevelButtonList levelButtonList;
        public BackgroundStatsList backgroundStatsList;

        public Skybox skybox;
        public List<CloudController> clouds;
        public MistController mist;
        public MoonController moon;
        public FogController fog;
        public BackgroundImageController backgroundImage;
        public Camera mainCamera;

        [Header("Peter can change these :D")]
        [Range(1, 25)]
        public int startingLevelNum = 1;

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);
            BCUtils.Helper.AssertIsNotNull(skyStatsGroup, skyButtonGroup, levelButtonList, backgroundStatsList, skybox, clouds, mist, moon, fog, backgroundImage, mainCamera);

            IList<ICloud> cloudList
                = clouds
                    .Select(cloud => (ICloud)cloud)
                    .ToList();

            ISkySetter skySetter
                = new SkySetter(
                    skybox,
                    cloudList,
                    mist,
                    moon,
                    fog);

            skyStatsGroup.Initialise();
            skyButtonGroup.Initialise(skySetter, skyStatsGroup.SkyStats);

            CloudInitialiser cloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(skybox.material.name, _updaterProvider.SlowerUpdater, startingLevelNum, mainCamera.aspect);

            IStaticData staticData = ApplicationModelProvider.ApplicationModel.DataProvider.StaticData;
            levelButtonList.Initialise(skyStatsGroup, skySetter, backgroundStatsList, backgroundImage, staticData, startingLevelNum);
        }
    }
}