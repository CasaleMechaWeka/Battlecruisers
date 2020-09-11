using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class LevelButtonList : MonoBehaviour
    {
        private const int NUM_OF_LEVELS = 25;

        public void Initialise(
            SkyStatsGroup skyStatsGroup, 
            ISkySetter skySetter, 
            BackgroundStatsList backgroundStatsList, 
            BackgroundImageController backgroundImage,
            IStaticData staticData, 
            int startingLevelNum)
        {
            BCUtils.Helper.AssertIsNotNull(skyStatsGroup, skySetter, backgroundStatsList, backgroundImage, staticData);

            LevelButtonController[] buttons = GetComponentsInChildren<LevelButtonController>();
            Assert.AreEqual(NUM_OF_LEVELS, buttons.Length);

            for (int i = 0; i < buttons.Length; ++i)
            {
                int levelNum = i + 1;

                ILevel level = staticData.Levels[levelNum - 1];
                ISkyStats skyStats = skyStatsGroup.GetSkyStats(level.SkyMaterialName);
                IBackgroundImageStats backgroundStats = backgroundStatsList.GetStats(levelNum);
                
                LevelButtonController button = buttons[i];
                button.Initialise(levelNum, skyStats, skySetter, backgroundStats, backgroundImage);

                if (startingLevelNum == levelNum)
                {
                    button.ChangeSky();
                }
            }
        }
    }
}