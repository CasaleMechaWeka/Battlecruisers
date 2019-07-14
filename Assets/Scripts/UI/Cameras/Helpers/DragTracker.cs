using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class DragTracker : MonoBehaviour, IDragHandler, IDragTracker
    {
        public event EventHandler<DragEventArgs> Drag;

        public void OnDrag(PointerEventData eventData)
        {
            Logging.Log(Tags.SWIPE_NAVIGATION, $"delta: {eventData.delta}");
            Drag?.Invoke(this, new DragEventArgs(eventData));
        }
    }
}