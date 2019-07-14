using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Cameras.Helpers
{
    // FELIX  Use in SwipeCTP
    public class SwipeTracker : MonoBehaviour, IDragHandler, ISwipeTracker
    {
        public event EventHandler<DragEventArgs> Drag;

        public void OnDrag(PointerEventData eventData)
        {
            Logging.Log(Tags.SWIPE_NAVIGATION, $"delta: {eventData.delta}");
            Drag?.Invoke(this, new DragEventArgs(eventData));
        }
    }
}