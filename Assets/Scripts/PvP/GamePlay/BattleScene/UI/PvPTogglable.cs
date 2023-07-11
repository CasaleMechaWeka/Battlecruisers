using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public class PvPTogglable : MonoBehaviourWrapper, IPvPTogglable, IPvPHighlightable
    {
        protected RectTransform _rectTransform;

        private const float DEFAULT_HIGHLIGHT_SIZE_MULTIPLIER = 1;

        protected virtual bool Disable => true;
        protected virtual MaskableGraphic Graphic => null;
        protected virtual CanvasGroup CanvasGroup => null;
        protected virtual bool ToggleVisibility => false;

        public float highlightSizeMultiplier;

        private bool _enabled;

        public event EventHandler EnabledChange;

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

                if (value)
                {
                    ShowEnabledState();
                }
                else
                {
                    ShowDisabledState();
                }

                if (ToggleVisibility)
                {
                    gameObject.SetActive(value);
                }

                EnabledChange?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void ShowEnabledState()
        {
            SetAlpha(PvPAlpha.ENABLED);
        }

        protected virtual void ShowDisabledState()
        {
            SetAlpha(PvPAlpha.DISABLED);
        }

        protected void SetAlpha(float alpha)
        {
            // Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}  alpha: {alpha}");

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

        public PvPHighlightArgs CreateHighlightArgs(IPvPHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForOnCanvasObject(_rectTransform, highlightSizeMultiplier);
        }

        // Need this for script enabled checkbox in editor.  Otherwise when adding script,
        // it is disabled by default, and none of my pointer events (OnPointerClick etc) are hit.
        void Update() { }
    }
}