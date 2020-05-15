using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class Highlightable : MonoBehaviour, IHighlightable
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