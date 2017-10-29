using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class TargetBounds : ITargetBounds
    {
        public Vector2 MinPosition { get; private set; }
        public Vector2 MaxPosition { get; private set; }

        public TargetBounds(Vector2 minPosition, Vector2 maxPosition)
        {
            MinPosition = minPosition;
            MaxPosition = maxPosition;
        }
    }
}
