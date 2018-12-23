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

        protected virtual bool Disable { get { return true; } }
        protected virtual Image Image { get { return null; } }
        protected virtual CanvasGroup CanvasGroup { get { return null; } }
        protected virtual bool ToggleVisibility { get { return false; } }

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

            if (highlightSizeMultiplier == default(float))
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