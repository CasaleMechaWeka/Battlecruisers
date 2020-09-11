using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    // FELIX  Move sky test related classes to own namespace :)
    public class SkyButtonController : MonoBehaviour
    {
        private ISkySetter _skySetter;
        private ISkyStats _skyStats;

        public Text skyName;

        public void Initialise(ISkySetter skySetter, ISkyStats skyStats)
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