using System;
using UnityCommon.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class DragEventArgs : EventArgs
    {
        public IPointerEventData PointerEventData { get; }

        public DragEventArgs(IPointerEventData pointerEventData)
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