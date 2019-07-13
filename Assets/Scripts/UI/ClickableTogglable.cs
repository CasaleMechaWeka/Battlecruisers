using BattleCruisers.Utils;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI
{
    public abstract class ClickableTogglable : Togglable,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        private bool _isPressed = false;

        protected virtual bool ShowPressedFeedback => true;
        protected virtual bool ShowHoverFeedback => true;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked();
        }

        protected abstract void OnClicked();

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;

            if (Enabled && ShowPressedFeedback)
            {
                SetAlpha(Alpha.PRESSED);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;

            if (Enabled && ShowPressedFeedback)
            {
                SetAlpha(Alpha.ENABLED);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Enabled && ShowHoverFeedback)
            {
                SetAlpha(Alpha.HOVER);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Enabled 
                && ShowHoverFeedback
                && !_isPressed)
            {
                SetAlpha(Alpha.ENABLED);
            }
        }
    }
}