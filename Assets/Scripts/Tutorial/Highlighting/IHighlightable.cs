using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    // FELIX  Delete :D
    public enum HighlightableType
    {
        InGame, OnCanvas
    }

    // FELIX  Delete :D
    public interface IHighlightable
    {
        ITransform Transform { get; }
        Vector2 PositionAdjustment { get; }
        Vector2 Size { get; }
        float SizeMultiplier { get; }
        HighlightableType HighlightableType { get; }
    }
}
