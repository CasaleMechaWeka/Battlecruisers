using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Common.Click
{
    public class ClickHandlerWrapper : MonoBehaviour, IPointerClickHandler
    {
        private ClickHandler _clickHandler;

        public IClickHandler GetClickHandler(float doubleClickThresholdInS = Constants.DEFAULT_DOUBLE_CLICK_THRESHOLD_IN_S)
        {
            if (_clickHandler == null)
            {
                _clickHandler = new ClickHandler(doubleClickThresholdInS);
            }
            return _clickHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_clickHandler != null)
            {
                _clickHandler.OnClick(Time.unscaledTime);
            }
        }
    }
}