using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface ITargetBoundsFinder
    {
        ITargetBounds FindTargetBounds(Vector2 sourcePosition, Vector2 targetPosition);
    }
}
