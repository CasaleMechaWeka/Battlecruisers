using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface ITargetBoundsFinder
    {
        IRange<Vector2> FindTargetBounds(Vector2 sourcePosition, Vector2 targetPosition);
    }
}
