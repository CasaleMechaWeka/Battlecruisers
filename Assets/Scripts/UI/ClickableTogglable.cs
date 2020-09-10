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
            Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            OnClicked();
        }

        protected abstract void OnClicked();

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            _isPressed = true;

            if (Enabled && ShowPressedFeedback)
            {
                ShowClickedState();
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            _isPressed = false;

            if (Enabled && ShowPressedFeedback)
            {
                if (_isHover)
                {
                    ShowHoverState();
                }
                else
                {
                    ShowEnabledState();
                }
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            _isHover = true;

            if (Enabled 
                && ShowHoverFeedback
                && !_isPressed)
            {
                ShowHoverState();
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");

            _isHover = false;

            if (Enabled 
                && ShowHoverFeedback
                && !_isPressed)
            {
                ShowEnabledState();
            }
        }

        protected virtual void ShowClickedState()
        {
            SetAlpha(Alpha.PRESSED);
        }

        protected virtual void ShowHoverState()
        {
            SetAlpha(Alpha.HOVER);
        }

        protected void Reset()
        {
            _isHover = false;
            _isPressed = false;
            // Resets alpha
            Enabled = Enabled;
        }
    }
}