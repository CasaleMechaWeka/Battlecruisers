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
        private bool _isHover = false;

        protected virtual bool ShowPressedFeedback => true;
        protected virtual bool ShowHoverFeedback => true;

        public void OnPointerClick(PointerEventData eventData)
        {
            Logging.Log(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            OnClicked();
        }

        protected abstract void OnClicked();

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Logging.Log(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            _isPressed = true;

            if (Enabled && ShowPressedFeedback)
            {
                SetAlpha(Alpha.PRESSED);
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Logging.Log(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            _isPressed = false;

            if (Enabled && ShowPressedFeedback)
            {
                if (_isHover)
                {
                    SetAlpha(Alpha.HOVER);
                }
                else
                {
                    SetAlpha(Alpha.ENABLED);
                }
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            Logging.Log(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            _isHover = true;

            if (Enabled 
                && ShowHoverFeedback
                && !_isPressed)
            {
                SetAlpha(Alpha.HOVER);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Logging.Log(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            _isHover = false;

            if (Enabled 
                && ShowHoverFeedback
                && !_isPressed)
            {
                SetAlpha(Alpha.ENABLED);
            }
        }
    }
}