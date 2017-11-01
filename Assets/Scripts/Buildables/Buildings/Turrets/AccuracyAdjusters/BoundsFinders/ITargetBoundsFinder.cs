using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public interface ITargetBoundsFinder
    {
        IRange<Vector2> FindTargetBounds(Vector2 sourcePosition, Vector2 targetPosition);
    }
}
