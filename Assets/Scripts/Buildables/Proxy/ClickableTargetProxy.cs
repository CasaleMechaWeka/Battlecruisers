using System;
using UnityEngine.EventSystems;

namespace BattleCruisers.Buildables.Proxy
{
    public class ClickableTargetProxy : TargetProxy, IClickableTargetProxy, IPointerClickHandler
    {
        public event EventHandler Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}