using BattleCruisers.Utils;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI
{
    public abstract class ClickableTogglable : Togglable,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked();
        }

        protected abstract void OnClicked();

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Enabled)
            {
                SetAlpha(Constants.PRESSED_UI_ALPHA);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Enabled)
            {
                SetAlpha(Constants.ENABLED_UI_ALPHA);
            }
        }
    }
}