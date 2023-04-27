using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IPvPAngleHelper
    {
        float FindAngle(Vector2 velocity, bool isSourceMirrored);
        float FindAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored);
    }
}
