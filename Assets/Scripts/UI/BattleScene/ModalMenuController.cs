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

		public CanvasGroupButton endGameButton, skipTutorialButton, resumeButton, retryButton;

		public void Initialise(ISingleSoundPlayer soundPlayer, bool isTutorial, IMainMenuManager menuManager)
		{
			Helper.AssertIsNotNull(endGameButton, skipTutorialButton, resumeButton, retryButton);
			Helper.AssertIsNotNull(soundPlayer, menuManager);

			_menuManager = menuManager;

            _canvas = GetComponent<Canvas>();
            Assert.IsNotNull(_canvas);

            endGameButton.Initialise(soundPlayer, _menuManager.QuitGame);
            skipTutorialButton.Initialise(soundPlayer, _menuManager.QuitGame);
            resumeButton.Initialise(soundPlayer, _menuManager.DismissMenu);
			retryButton.Initialise(soundPlayer, _menuManager.RetryLevel);

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

		void Update()
		{
			// IPAD  Adapt for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				if (_canvas.enabled)
                {
					_menuManager.DismissMenu();
                }
				else
                {
					_menuManager.ShowMenu();
                }
			}
		}

		public void ShowMenu()
		{
			_canvas.enabled = true;
		}

		public void HideMenu()
		{
			_canvas.enabled = false;
		}
	}
}
