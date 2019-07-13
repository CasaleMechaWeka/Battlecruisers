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
                SetAlpha(Alpha.PRESSED);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Enabled)
            {
                SetAlpha(Alpha.ENABLED);
            }
        }
    }
}