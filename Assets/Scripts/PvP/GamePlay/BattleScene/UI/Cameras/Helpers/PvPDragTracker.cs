using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPDragTracker : MonoBehaviour,
        IPvPDragTracker,
        IDragHandler,
        IBeginDragHandler,
        IEndDragHandler
    {
        public event EventHandler<PvPDragEventArgs> DragStart;
        public event EventHandler<PvPDragEventArgs> Drag;
        public event EventHandler<PvPDragEventArgs> DragEnd;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            // Logging.VerboseMethod(Tags.SWIPE_NAVIGATION);
            DragStart?.Invoke(this, new PvPDragEventArgs(new PvPPointerEventDataBC(eventData)));
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            // Logging.Verbose(Tags.SWIPE_NAVIGATION, $"delta: {eventData.delta}");
            Drag?.Invoke(this, new PvPDragEventArgs(new PvPPointerEventDataBC(eventData)));
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            // Logging.VerboseMethod(Tags.SWIPE_NAVIGATION);
            DragEnd?.Invoke(this, new PvPDragEventArgs(new PvPPointerEventDataBC(eventData)));
        }
    }
}