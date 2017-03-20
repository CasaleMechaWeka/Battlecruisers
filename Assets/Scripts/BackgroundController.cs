using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.UI;

namespace BattleCruisers
{
	public class BackgroundController : MonoBehaviour 
	{
		public event EventHandler BackgroundClicked;

		void OnMouseDown()
		{
			if (BackgroundClicked != null)
			{
				BackgroundClicked.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
