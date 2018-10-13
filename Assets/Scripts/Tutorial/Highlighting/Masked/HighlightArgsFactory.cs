using System;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class HighlightArgsFactory : IHighlightArgsFactory
    {
        public HighlightArgs CreateForOnCanvasObject(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return new HighlightArgs(corners[0], rectTransform.sizeDelta);
        }

        public HighlightArgs CreateForInGameObject(Vector2 worldPosition, Vector2 worldSize)
        {
            // FELIX :D
            throw new NotImplementedException();
        }
    }
}