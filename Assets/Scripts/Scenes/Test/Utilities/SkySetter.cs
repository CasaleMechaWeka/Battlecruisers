using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class SkySetter : ISkySetter
    {
        private readonly Skybox _skybox;
        private readonly IList<ICloud> _clouds;
        private readonly MistController _mist;
        private readonly MoonController _moon;
        private readonly FogController _fog;

        public SkySetter(
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