using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPTogglableDragTracker : PvPDragTracker
    {
        private IPvPBroadcastingFilter _enabledFilter;

        public void Initialise(IPvPBroadcastingFilter enabledFilter)
        {
            Assert.IsNotNull(enabledFilter);
            _enabledFilter = enabledFilter;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (_enabledFilter.IsMatch)
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (_enabledFilter.IsMatch)
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (_enabledFilter.IsMatch)
            {
                base.OnEndDrag(eventData);
            }
        }
    }
}