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
        protected virtual MaskableGraphic Graphic => null;
        protected virtual CanvasGroup CanvasGroup => null;
        protected virtual bool ToggleVisibility => false;

        public float highlightSizeMultiplier;

        private bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;

                if (Disable)
                {
                    enabled = value;
                }

                float alpha = value ? Alpha.ENABLED : Alpha.DISABLED;
                SetAlpha(alpha);

                if (ToggleVisibility)
                {
                    gameObject.SetActive(value);
                }
            }
        }

        protected void SetAlpha(float alpha)
        {
            if (Graphic != null)
            {
                Color color = Graphic.color;
                color.a = alpha;
                Graphic.color = color;
            }

            if (CanvasGroup != null)
            {
                CanvasGroup.alpha = alpha;
            }
        }

        public virtual void Initialise()
        {
            _enabled = true;
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