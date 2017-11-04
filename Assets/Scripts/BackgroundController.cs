using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers
{
    public class BackgroundController : MonoBehaviour, IPointerClickHandler, IClickable
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
