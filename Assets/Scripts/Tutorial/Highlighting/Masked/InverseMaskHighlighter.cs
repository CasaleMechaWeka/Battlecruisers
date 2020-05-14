using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class InverseMaskHighlighter : MonoBehaviour, IMaskHighlighter
    {
        private RectTransform _rectTransform;

        public void Initialise()
        {
            _rectTransform = transform.Parse<RectTransform>();
        }

        public void Highlight(HighlightArgs args)
        {
            _rectTransform.position = args.CenterPosition;
            _rectTransform.sizeDelta = args.Size;

            gameObject.SetActive(true);
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
        }
    }
}