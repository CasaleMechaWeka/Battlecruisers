using BattleCruisers.Tutorial.Highlighting.Masked;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Arrows
{
    public enum ArrowDirection
    {
        NorthWest,
        North,  // For when highlightable is exactly in the center of the screen
        NorthEast,
        SouthEast,
        SouthWest
    }

    public interface IArrowCalculator
    {
        ArrowDirection FindArrowDirection(Vector2 highlightableCenterPosition);
        Vector2 FindArrowDirectionVector(Vector2 arrowHead, Vector2 highlightableCenterPosition);
        Vector2 FindArrowHeadPosition(HighlightArgs args, ArrowDirection direction);
    }
}