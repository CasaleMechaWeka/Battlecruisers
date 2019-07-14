using System;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class DragEventArgs : EventArgs
    {
        public PointerEventData PointerEventData { get; }

        public DragEventArgs(PointerEventData pointerEventData)
        {
            Assert.IsNotNull(pointerEventData);
            PointerEventData = pointerEventData;
        }
    }

    public interface IDragTracker
    {
        event EventHandler<DragEventArgs> DragStart;
        event EventHandler<DragEventArgs> Drag;
        event EventHandler<DragEventArgs> DragEnd;
    }
}