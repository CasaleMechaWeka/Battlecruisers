using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class LevelButtonList : MonoBehaviour
    {
        public Camera mainCamera;

        public async Task InitialiseAsync(
            SkyStatsGroup skyStatsGroup, 
            ISkySetter skySetter, 
            BackgroundStatsList backgroundStatsList, 
            BackgroundImageController backgroundImage,
            IStaticData staticData, 
            int startingLevelNum)
        {
            Assert.IsNotNull(mainCamera);
            BCUtils.Helper.AssertIsNotNull(skyStatsGroup, skySetter, backgroundStatsList, backgroundImage, staticData);

            IBackgroundImageCalculator calculator = new BackgroundImageCalculator();

            CloudLevelButtonController[] buttons = GetComponentsInChildren<CloudLevelButtonController>();
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, buttons.Length);

            for (int i = 0; i < buttons.Length; ++i)
            {
                int levelNum = i + 1;

                ILevel level = staticData.Levels[levelNum - 1];
                ISkyStats skyStats = skyStatsGroup.GetSkyStats(level.SkyMaterialName);
                IPrefabContainer<BackgroundImageStats> backgroundStats = await backgroundStatsList.GetStatsAsync(levelNum);
                
                CloudLevelButtonController button = buttons[i];
                button.Initialise(levelNum, skyStats, skySetter, backgroundStats, backgroundImage, mainCamera.aspect, calculator);

                if (startingLevelNum == levelNum)
                {
                    button.ChangeSky();
                }
            }
        }
    }
}