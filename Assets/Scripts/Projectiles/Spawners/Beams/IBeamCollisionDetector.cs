using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners.Beams
{
    public interface IBeamCollisionDetector
    {
        IBeamCollision FindCollision(Vector2 source, float angleInDegrees, bool isSourceMirrored);
    }
}
