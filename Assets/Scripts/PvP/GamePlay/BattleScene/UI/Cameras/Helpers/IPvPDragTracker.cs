using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPDragEventArgs : EventArgs
    {
        public IPointerEventData PointerEventData { get; }

        public PvPDragEventArgs(IPointerEventData pointerEventData)
        {
            Assert.IsNotNull(pointerEventData);
            PointerEventData = pointerEventData;
        }
    }

    public interface IPvPDragTracker
    {
        event EventHandler<PvPDragEventArgs> DragStart;
        event EventHandler<PvPDragEventArgs> Drag;
        event EventHandler<PvPDragEventArgs> DragEnd;
    }
}