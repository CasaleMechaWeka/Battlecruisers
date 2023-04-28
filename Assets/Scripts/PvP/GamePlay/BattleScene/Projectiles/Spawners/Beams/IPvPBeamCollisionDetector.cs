using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public interface IPvPBeamCollisionDetector
    {
        IPvPBeamCollision FindCollision(Vector2 source, float angleInDegrees, bool isSourceMirrored);
    }
}
