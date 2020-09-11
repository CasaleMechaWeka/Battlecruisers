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
        private ISkyStats _skyStats;
        private IBackgroundImageStats _backgroundStats;
        private BackgroundImageController _backgroundImage;

        public Text levelNumText;

        public void Initialise(
            int levelNum, 
            ISkyStats skyStats,
            ISkySetter skySetter,
            IBackgroundImageStats backgroundStats,
            BackgroundImageController backgroundImage)
        {
            Assert.IsNotNull(levelNumText);
            BCUtils.Helper.AssertIsNotNull(skyStats, skySetter, backgroundStats, backgroundImage);

            _skyStats = skyStats;
            _skySetter = skySetter;
            _backgroundStats = backgroundStats;
            _backgroundImage = backgroundImage;
            levelNumText.text = levelNum.ToString();
        }

        public void ChangeSky()
        {
            _skySetter.SetSky(_skyStats);
            _backgroundImage.Initialise(_backgroundStats);
        }
    }
}