using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPClickHandlerWrapper : MonoBehaviour, IPointerClickHandler
    {
        private PvPClickHandler _clickHandler;

        public IPvPClickHandler GetClickHandler(float doubleClickThresholdInS = Constants.DEFAULT_DOUBLE_CLICK_THRESHOLD_IN_S)
        {
            if (_clickHandler == null)
            {
                _clickHandler = new PvPClickHandler(doubleClickThresholdInS);
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