using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    // FELIX  Rename, up a namespace
    public class MaskHighlightable : MonoBehaviour, IHighlightable
    {
        private RectTransform _rectTransform;

        public float sizeMultiplier;

        public void Initialise()
        {
            _rectTransform = transform.Parse<RectTransform>();
        }

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForOnCanvasObject(_rectTransform, sizeMultiplier);
        }
    }
}