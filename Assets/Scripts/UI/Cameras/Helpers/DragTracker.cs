using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class DragTracker : MonoBehaviour, 
        IDragTracker,
        IDragHandler,
        IBeginDragHandler,
        IEndDragHandler
    {
        public event EventHandler<DragEventArgs> DragStart;
        public event EventHandler<DragEventArgs> Drag;
        public event EventHandler<DragEventArgs> DragEnd;

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragStart?.Invoke(this, new DragEventArgs(eventData));
        }

        public void OnDrag(PointerEventData eventData)
        {
            Logging.Log(Tags.SWIPE_NAVIGATION, $"delta: {eventData.delta}");
            Drag?.Invoke(this, new DragEventArgs(eventData));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragEnd?.Invoke(this, new DragEventArgs(eventData));
        }
    }
}