using UnityEngine;

namespace BattleCruisers.Tutorial
{
    public enum HighlightableType
    {
        InGame, OnCanvas
    }

    public interface IHighlightable
    {
        Transform Transform { get; }
        Vector2 Size { get; }
        HighlightableType Type { get; }
    }
}
