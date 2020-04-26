using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class SkyStatsController : MonoBehaviour, ISkyStats
    {
        public Material skyboxMaterial;
        public Material SkyMaterial => skyboxMaterial;

        public CloudMovementSpeed movementSpeed = CloudMovementSpeed.Fast;
        public float HorizontalMovementSpeedInMPerS { get; private set; }

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

            HorizontalMovementSpeedInMPerS = ConvertMovementSpeed(movementSpeed);
        }

        private static float ConvertMovementSpeed(CloudMovementSpeed movementSpeed)
        {
            switch (movementSpeed)
            {
                case CloudMovementSpeed.Fast:
                    return 0.75f;
                case CloudMovementSpeed.Slow:
                default:
                    return 0.5f;
            }
        }
    }
}