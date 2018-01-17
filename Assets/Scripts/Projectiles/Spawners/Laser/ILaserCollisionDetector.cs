using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners.Laser
{
    public interface ILaserCollisionDetector
    {
        ILaserCollision FindCollision(Vector2 source, float angleInDegrees, bool isSourceMirrored);
    }
}
