using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public interface IPvPTargetBoundsFinder
    {
        IRange<Vector2> FindTargetBounds(Vector2 sourcePosition, Vector2 targetPosition);
    }
}
