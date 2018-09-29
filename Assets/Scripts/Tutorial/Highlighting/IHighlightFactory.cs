using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlightFactory
    {
        IHighlight CreateInGameHighlight(float radiusInM, Vector2 position, bool usePulsingAnimation);
        IHighlight CreateOnCanvasHighlight(float radiusInPixels, Transform parentTransform, Vector2 positionAdjustment);
    }
}
