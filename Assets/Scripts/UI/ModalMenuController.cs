using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI
{
	public enum UserAction
	{
		Dismissed, Quit
	}

	public class ModalMenuController : MonoBehaviour 
	{
		private MenuDismissed _onMenuDismissed;

		public Canvas canvas;

		public delegate void MenuDismissed(UserAction UserAction);

		void Start() 
		{
			HideMenu();
		}

		public void ShowMenu(MenuDismissed onMenuDismissed)
		{
			_onMenuDismissed = onMenuDismissed;
			canvas.gameObject.SetActive(true);
		}

		void Update()
		{
			// FELIX  Adapt for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				DismissMenu(UserAction.Dismissed);
			}
		}

		private void DismissMenu(UserAction userAction)
		{
			Assert.IsNotNull(_onMenuDismissed);

			HideMenu();
			_onMenuDismissed.Invoke(userAction);
		}

		private void HideMenu()
		{
			canvas.gameObject.SetActive(false);
		}
	}
}
