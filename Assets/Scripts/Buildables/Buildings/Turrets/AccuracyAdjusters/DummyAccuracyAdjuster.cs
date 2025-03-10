using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    /// <summary>
    /// Null object
    /// </summary>
    public class DummyAccuracyAdjuster : IAccuracyAdjuster
    {
        public float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            return idealFireAngle;
        }
    }
}
