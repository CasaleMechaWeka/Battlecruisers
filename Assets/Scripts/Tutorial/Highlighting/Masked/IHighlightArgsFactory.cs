using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public interface IHighlightArgsFactory
    {
        HighlightArgs CreateForOnCanvasObject(RectTransform rectTransform);
        HighlightArgs CreateForInGameObject(Vector2 position, Vector2 size);
    }
}