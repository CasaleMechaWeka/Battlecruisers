using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlightArgsFactory
    {
        HighlightArgs CreateForOnCanvasObject(RectTransform rectTransform, float sizeMultiplier);
        HighlightArgs CreateForInGameObject(Vector2 objectWorldPosition, Vector2 objectWorldSize);
    }
}