using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public enum HighlightableType
    {
        InGame, OnCanvas
    }

    public interface IHighlightable
    {
        ITransform Transform { get; }
        Vector2 PositionAdjustment { get; }
        Vector2 Size { get; }
        float SizeMultiplier { get; }
        HighlightableType HighlightableType { get; }
    }
}
