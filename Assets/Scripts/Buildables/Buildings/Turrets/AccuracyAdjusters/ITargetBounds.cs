using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    /// <summary>
    /// The positions which if hit count as having "hit" a target, for accuracy purposes.
    /// </summary>
    public interface ITargetBounds
    {
        Vector2 MinPosition { get; }
        Vector2 MaxPosition { get; }
    }
}
