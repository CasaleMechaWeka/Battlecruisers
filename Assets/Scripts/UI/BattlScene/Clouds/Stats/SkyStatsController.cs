using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class SkyStatsController : MonoBehaviour, ISkyStats
    {
        public Material skyboxMaterial;
        public Material SkyMaterial => skyboxMaterial;

        public float movementSpeedInMPerS = 5;
        public float HorizontalMovementSpeedInMPerS => movementSpeedInMPerS;

        public Color cloudColour = Color.white;
        public Color CloudColour => cloudColour;

        public Color mistColour = Color.white;
        public Color MistColour => mistColour;

        public float cloudHeight = 28;
        public float Height => cloudHeight;

        public bool flipClouds = false;
        public bool FlipClouds => flipClouds;

        public IMoonStats MoonStats { get; private set; }

        public void Initialise()
        {
            MoonStats = GetComponentInChildren<MoonStatsController>();
            Assert.IsNotNull(MoonStats);
        }
    }
}