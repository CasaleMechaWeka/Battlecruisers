using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    /// <summary>
    /// Null object
    /// </summary>
    public class PvPDummyAccuracyAdjuster : IAccuracyAdjuster
    {
        public float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            return idealFireAngle;
        }
    }
}
