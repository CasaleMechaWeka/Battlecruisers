using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class LevelButtonController : MonoBehaviour
    {
        private ISkySetter _skySetter;
        private ISkyStats _levelSkyStats;

        public Text levelNumText;

        public void Initialise(int levelNum, SkyStatsGroup skyStats, ISkySetter skySetter, IStaticData staticData)
        {
            Assert.IsNotNull(levelNumText);
            BCUtils.Helper.AssertIsNotNull(skyStats, skySetter, staticData);

            _skySetter = skySetter;
            levelNumText.text = levelNum.ToString();

            ILevel level = staticData.Levels[levelNum - 1];
            _levelSkyStats = skyStats.GetSkyStats(level.SkyMaterialName);
        }

        public void ChangeSky()
        {
            _skySetter.SetSky(_levelSkyStats);
        }
    }
}