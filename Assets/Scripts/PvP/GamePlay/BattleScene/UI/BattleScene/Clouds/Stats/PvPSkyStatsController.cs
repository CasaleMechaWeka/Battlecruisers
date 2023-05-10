using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public class PvPSkyStatsController : MonoBehaviour, IPvPSkyStats
    {
        public IPvPMoonStats MoonStats { get; private set; }

        public Material skyboxMaterial;
        public Material SkyMaterial => skyboxMaterial;

        public float movementSpeedInMPerS = 5;
        public float HorizontalMovementSpeedInMPerS => movementSpeedInMPerS;

        public bool flipClouds = false;
        public bool FlipClouds => flipClouds;

        public Color cloudColour = Color.white;
        public Color CloudColour => cloudColour;

        public float cloudYPosition;
        public float CloudYPosition => cloudYPosition;

        public float cloudZPosition = 290;
        public float CloudZPosition => cloudZPosition;

        public Color mistColour = Color.white;
        public Color MistColour => mistColour;

        public float mistYPosition = 0;
        public float MistYPosition => mistYPosition;

        public float mistZPosition = 0;
        public float MistZPosition => mistZPosition;

        [SerializeField]
        private Color _fogColour;
        public Color FogColour => _fogColour;

        public void Initialise()
        {
            MoonStats = GetComponentInChildren<PvPMoonStatsController>();
            Assert.IsNotNull(MoonStats);
        }
    }
}