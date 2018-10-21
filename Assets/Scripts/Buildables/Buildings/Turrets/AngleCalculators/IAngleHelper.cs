using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IAngleHelper
    {
        /// <returns>Angle between -180 and 180 degrees.</returns>
        float FindAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored);

        /// <returns>Angle between -180 and 180 degrees.</returns>
        float FindAngle(Vector2 velocity, bool isSourceMirrored);
    }
}
