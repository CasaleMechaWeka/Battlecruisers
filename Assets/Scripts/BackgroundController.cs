using BattleCruisers.UI.BattleScene;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers
{
	public class BackgroundController : MonoBehaviour, IPointerClickHandler
	{
		public event EventHandler BackgroundClicked;

		public void OnPointerClick(PointerEventData eventData)
		{
			if (BackgroundClicked != null)
			{
				BackgroundClicked.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
