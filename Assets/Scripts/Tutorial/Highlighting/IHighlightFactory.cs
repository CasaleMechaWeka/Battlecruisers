using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlightFactory
    {
        IHighlight CreateInGameHighlight(float radiusInM, Vector2 position);
        IHighlight CreateOnCanvasHighlight(float radiusInPixels, Transform parentTransform);
    }
}
