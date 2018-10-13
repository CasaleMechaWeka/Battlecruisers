using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public interface IHighlightArgsFactory
    {
        HighlightArgs CreateForOnCanvasObject(RectTransform rectTransform);
        HighlightArgs CreateForInGameObject(Vector2 worldPosition, Vector2 worldSize);
    }
}