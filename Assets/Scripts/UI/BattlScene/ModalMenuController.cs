using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class ModalMenuController : MonoBehaviour, IModalMenu
    {
		private Canvas _canvas;
		private MenuDismissed _onMenuDismissed;

		public void Initialise(ISoundPlayer soundPlayer, bool isTutorial)
		{
            _canvas = GetComponent<Canvas>();
            Assert.IsNotNull(_canvas);

            ActionButton endGameButton = transform.FindNamedComponent<ActionButton>("ModalMenuPanel/EndGameButton");
            endGameButton.Initialise(soundPlayer, Quit);

            ActionButton skipTutorialButton = transform.FindNamedComponent<ActionButton>("ModalMenuPanel/SkipTutorialButton");
            skipTutorialButton.Initialise(soundPlayer, Quit);

            ActionButton cancelButton = transform.FindNamedComponent<ActionButton>("ModalMenuPanel/CancelButton");
            cancelButton.Initialise(soundPlayer, Cancel);

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
			_canvas.gameObject.SetActive(true);
		}

		void Update()
		{
			// IPAD  Adapt for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
                Cancel();
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
			_canvas.gameObject.SetActive(false);
		}
	}
}
