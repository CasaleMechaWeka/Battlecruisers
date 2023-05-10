using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPDragEventArgs : EventArgs
    {
        public IPvPPointerEventData PointerEventData { get; }

        public PvPDragEventArgs(IPvPPointerEventData pointerEventData)
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