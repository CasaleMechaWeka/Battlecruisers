using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
    public enum UserAction
	{
		Dismissed, Quit
	}

	public class ModalMenuController : MonoBehaviour 
	{
		private MenuDismissed _onMenuDismissed;

		public Canvas canvas;
        public Button endGameButton, skipTutorialButton;

		public delegate void MenuDismissed(UserAction UserAction);

		public void Initialise(bool isTutorial)
		{
            Helper.AssertIsNotNull(canvas, endGameButton, skipTutorialButton);

            if (isTutorial)
            {
                Destroy(endGameButton.gameObject);
            }
            else
            {
                Destroy(skipTutorialButton.gameObject);
            }

			HideMenu();
		}

		public void ShowMenu(MenuDismissed onMenuDismissed)
		{
			_onMenuDismissed = onMenuDismissed;
			canvas.gameObject.SetActive(true);
		}

		void Update()
		{
			// IPAD  Adapt for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				DismissMenu(UserAction.Dismissed);
			}
		}

		public void Cancel()
		{
			DismissMenu(UserAction.Dismissed);
		}

		public void Quit()
		{
			DismissMenu(UserAction.Quit);
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
