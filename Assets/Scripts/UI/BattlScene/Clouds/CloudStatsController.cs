using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudStatsController : MonoBehaviour
    {
        [Tooltip("Cloud density is the total area of all clouds divided by the area clouds are allowed to spawn in.  0 means no clouds.  1 means some clouds.  2 means more clouds, etc")]
        public float cloudDensityAsFraction = 0.5f;

        public CloudDensity legacyDensity = CloudDensity.VeryHigh;
        public CloudMovementSpeed movementSpeed = CloudMovementSpeed.Fast;
    }
}