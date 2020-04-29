using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class SkyButtonController : MonoBehaviour
    {
        private Skybox _skybox;
        private ISkyStats _skyStats;
        private IList<ICloud> _clouds;
        private MistController _mist;
        private MoonController _moon;

        public Text skyName;

        public void Initialise(
            Skybox skybox, 
            ISkyStats skyStats, 
            IList<ICloud> clouds, 
            MistController mist,
            MoonController moon)
        {
            BCUtils.Helper.AssertIsNotNull(skybox, skyStats, clouds, mist, moon);
            Assert.IsNotNull(skyName);

            _skybox = skybox;
            _skyStats = skyStats;
            _clouds = clouds;
            _mist = mist;
            _moon = moon;

            skyName.text = _skyStats.SkyMaterial.name;
        }

        public void SelectSky()
        {
            _skybox.material = _skyStats.SkyMaterial;

            foreach (ICloud cloud in _clouds)
            {
                cloud.Initialise(_skyStats);
            }

            _mist.Initialse(_skyStats.MistColour);
            _moon.Initialise(_skyStats.MoonStats);
        }
    }
}