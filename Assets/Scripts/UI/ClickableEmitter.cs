using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI
{
    public class ClickableEmitter : MonoBehaviour, IClickableEmitter, IPointerClickHandler
    {
        public event EventHandler Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
    }
}