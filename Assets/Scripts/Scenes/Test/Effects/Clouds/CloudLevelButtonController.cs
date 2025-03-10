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
        private ISkyStats _skyStats;
        private IPrefabContainer<BackgroundImageStats> _backgroundStats;
        private BackgroundImageController _backgroundImage;
        private float _cameraAspectRatio;
        private IBackgroundImageCalculator _calculator;

        public Text levelNumText;

        public void Initialise(
            int levelNum, 
            ISkyStats skyStats,
            ISkySetter skySetter,
            IPrefabContainer<BackgroundImageStats> backgroundStats,
            BackgroundImageController backgroundImage,
            float cameraAspectRatio,
            IBackgroundImageCalculator calculator)
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