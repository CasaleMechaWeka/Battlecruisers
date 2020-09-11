using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class SkySetter : MonoBehaviour, ISkySetter
    {
        private Skybox _skybox;
        private IList<ICloud> _clouds;
        private MistController _mist;
        private MoonController _moon;
        private FogController _fog;

        public void Initialise(
            Skybox skybox,
            IList<ICloud> clouds,
            MistController mist,
            MoonController moon,
            FogController fog)
        {
            BCUtils.Helper.AssertIsNotNull(skybox, clouds, mist, moon, fog);

            _skybox = skybox;
            _clouds = clouds;
            _mist = mist;
            _moon = moon;
            _fog = fog;
        }

        public void SetSky(ISkyStats skyStats)
        {
            _skybox.material = skyStats.SkyMaterial;

            foreach (ICloud cloud in _clouds)
            {
                cloud.Initialise(skyStats);
            }

            _mist.Initialise(skyStats);
            _moon.Initialise(skyStats.MoonStats);
            _fog.Initialise(skyStats.FogColour);
        }
    }
}