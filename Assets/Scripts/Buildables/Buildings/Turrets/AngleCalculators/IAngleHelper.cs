using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IAngleHelper
    {
        float FindAngle(Vector2 velocity);
        float FindAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored);
    }
}
