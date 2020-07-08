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

		public ActionButton endGameButton, skipTutorialButton, resumeButton, retryButton;

		public void Initialise(ISingleSoundPlayer soundPlayer, bool isTutorial)
		{
			Helper.AssertIsNotNull(endGameButton, skipTutorialButton, resumeButton, retryButton);
			Assert.IsNotNull(soundPlayer);

            _canvas = GetComponent<Canvas>();
            Assert.IsNotNull(_canvas);

            endGameButton.Initialise(soundPlayer, Quit);
            skipTutorialButton.Initialise(soundPlayer, Quit);
            resumeButton.Initialise(soundPlayer, Cancel);
			retryButton.Initialise(soundPlayer, Retry);

            if (isTutorial)
            {
                Destroy(endGameButton.gameObject);
                Destroy(retryButton.gameObject);
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

		public void HideMenu()
		{
			_canvas.gameObject.SetActive(false);
		}

		private void Cancel()
		{
			_menuManager.DismissMenu();
		}

		private void Quit()
		{
			_menuManager.QuitGame();
		}

		private void Retry()
		{
			_menuManager.RetryLevel();
		}
	}
}
