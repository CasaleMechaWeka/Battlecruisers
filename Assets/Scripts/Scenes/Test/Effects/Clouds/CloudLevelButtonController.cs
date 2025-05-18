using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class CloudLevelButtonController : MonoBehaviour
    {
        private ISkySetter _skySetter;
        private SkyStatsController _skyStats;
        private PrefabContainer<BackgroundImageStats> _backgroundStats;
        private BackgroundImageController _backgroundImage;
        private float _cameraAspectRatio;
        private BackgroundImageCalculator _calculator;

        public Text levelNumText;

        public void Initialise(
            int levelNum,
            SkyStatsController skyStats,
            ISkySetter skySetter,
            PrefabContainer<BackgroundImageStats> backgroundStats,
            BackgroundImageController backgroundImage,
            float cameraAspectRatio,
            BackgroundImageCalculator calculator)
        {
            Assert.IsNotNull(levelNumText);
            BCUtils.Helper.AssertIsNotNull(skyStats, skySetter, backgroundStats, backgroundImage, calculator);

            _skyStats = skyStats;
            _skySetter = skySetter;
            _backgroundStats = backgroundStats;
            _backgroundImage = backgroundImage;
            _cameraAspectRatio = cameraAspectRatio;
            _calculator = calculator;
            levelNumText.text = levelNum.ToString();
        }

        public void ChangeSky()
        {
            _skySetter.SetSky(_skyStats);
            _backgroundImage.Initialise(_backgroundStats, _cameraAspectRatio, _calculator);
        }
    }
}