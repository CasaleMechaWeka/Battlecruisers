using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudStatsController : MonoBehaviour
    {
        public float minZPosition = 0;
        public float maxZPosition = 20;
        public CloudDensity density = CloudDensity.VeryHigh;
        public CloudMovementSpeed movementSpeed = CloudMovementSpeed.Fast;
    }
}