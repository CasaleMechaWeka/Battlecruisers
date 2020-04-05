using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class SkyStatsController : MonoBehaviour, ISkyStats
    {
        public Material skyboxMaterial;
        public Material SkyMaterial => skyboxMaterial;

        public CloudMovementSpeed movementSpeed = CloudMovementSpeed.Fast;
        public float HorizontalMovementSpeedInMPerS { get; private set; }

        public Color frontCloudColour = Color.white;
        public Color FrontCloudColour => frontCloudColour;

        public Color backCloudColour = Color.white;
        public Color BackCloudColour => backCloudColour;

        public void Initialise()
        {
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