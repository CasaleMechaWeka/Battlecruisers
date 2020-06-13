using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class ModalMenuController : MonoBehaviour, IModalMenu
    {
		private Canvas _canvas;
		private IMainMenuManager _menuManager;

		public void Initialise(ISingleSoundPlayer soundPlayer, bool isTutorial)
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

		public void ShowMenu(IMainMenuManager menuManager)
		{
			_menuManager = menuManager;
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
			_menuManager.DismissMenu();
		}

		public void Quit()
		{
			_menuManager.QuitGame();
		}

		public void HideMenu()
		{
			_canvas.gameObject.SetActive(false);
		}
	}
}
