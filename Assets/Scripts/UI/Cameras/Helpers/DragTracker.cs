using BattleCruisers.Utils;
using System;
using BattleCruisers.Utils.PlatformAbstractions;
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

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            Logging.VerboseMethod(Tags.SWIPE_NAVIGATION);
            DragStart?.Invoke(this, new DragEventArgs(new PointerEventDataBC(eventData)));
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            Logging.Verbose(Tags.SWIPE_NAVIGATION, $"delta: {eventData.delta}");
            Drag?.Invoke(this, new DragEventArgs(new PointerEventDataBC(eventData)));
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            Logging.VerboseMethod(Tags.SWIPE_NAVIGATION);
            DragEnd?.Invoke(this, new DragEventArgs(new PointerEventDataBC(eventData)));
        }
    }
}