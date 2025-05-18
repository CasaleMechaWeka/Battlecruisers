using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class SkyButtonController : MonoBehaviour
    {
        private ISkySetter _skySetter;
        private SkyStatsController _skyStats;

        public Text skyName;

        public void Initialise(ISkySetter skySetter, SkyStatsController skyStats)
        {
            BCUtils.Helper.AssertIsNotNull(skySetter, skyStats);
            Assert.IsNotNull(skyName);

            _skySetter = skySetter;
            _skyStats = skyStats;

            skyName.text = _skyStats.SkyMaterial.name;
        }

        public void SelectSky()
        {
            _skySetter.SetSky(_skyStats);
        }
    }
}