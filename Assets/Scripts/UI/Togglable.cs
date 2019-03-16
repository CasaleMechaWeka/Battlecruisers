using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI
{
    public class Togglable : MonoBehaviourWrapper, ITogglable, IMaskHighlightable
    {
        protected RectTransform _rectTransform;

        private const float DEFAULT_HIGHLIGHT_SIZE_MULTIPLIER = 1;

        protected virtual bool Disable => true;
        protected virtual Image Image => null;
        protected virtual CanvasGroup CanvasGroup => null;
        protected virtual bool ToggleVisibility => false;

        public float highlightSizeMultiplier;

        public bool Enabled
        {
            set
            {
                if (Disable)
                {
                    enabled = value;
                }

                if (Image != null)
                {
                    Color color = Image.color;
                    color.a = value ? Constants.ENABLED_UI_ALPHA : Constants.DISABLED_UI_ALPHA;
                    Image.color = color;
                }

                if (CanvasGroup != null)
                {
                    CanvasGroup.alpha = value ? Constants.ENABLED_UI_ALPHA : Constants.DISABLED_UI_ALPHA;
                }

                if (ToggleVisibility)
                {
                    gameObject.SetActive(value);
                }
            }
        }

        public virtual void Initialise()
        {
            _rectTransform = transform.Parse<RectTransform>();

            if (highlightSizeMultiplier == default)
            {
                highlightSizeMultiplier = DEFAULT_HIGHLIGHT_SIZE_MULTIPLIER;
            }
        }

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForOnCanvasObject(_rectTransform, highlightSizeMultiplier);
        }
    }
}