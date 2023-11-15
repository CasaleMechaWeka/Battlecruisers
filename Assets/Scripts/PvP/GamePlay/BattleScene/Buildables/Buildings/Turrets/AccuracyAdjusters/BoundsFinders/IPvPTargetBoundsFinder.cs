using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public interface IPvPTargetBoundsFinder
    {
        IPvPRange<Vector2> FindTargetBounds(Vector2 sourcePosition, Vector2 targetPosition);
    }
}
