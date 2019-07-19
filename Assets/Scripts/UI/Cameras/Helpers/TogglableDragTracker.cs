using BattleCruisers.UI.Filters;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class TogglableDragTracker : DragTracker
    {
        private IBroadcastingFilter _enabledFilter;

        public void Initialise(IBroadcastingFilter enabledFilter)
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