using BattleCruisers.UI;
using System;
using UnityEngine.EventSystems;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationDismissButton : Togglable, 
        IExplanationDismissButton, 
        IPointerClickHandler
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