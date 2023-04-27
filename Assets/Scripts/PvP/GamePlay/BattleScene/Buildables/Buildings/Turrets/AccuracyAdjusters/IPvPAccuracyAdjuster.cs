using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IPvPAccuracyAdjuster
    {
        float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored);
    }
}
