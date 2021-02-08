using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public class ModalMenuController : MonoBehaviour, IModalMenu
	{
		private Canvas _canvas;
		private IMainMenuManager _menuManager;

		public Panel buttonsPanel;
		public CanvasGroupButton endGameButton, skipTutorialButton, resumeButton, retryButton, settingsButton;
		public InGameSettingsPanel settingsPanel;

		private ISettableBroadcastingProperty<bool> _isVisible;
		public IBroadcastingProperty<bool> IsVisible { get; private set; }

        public void Initialise(
			ISingleSoundPlayer soundPlayer, 
			bool isTutorial, 
			IMainMenuManager menuManager,
			ISettingsManager settingsManager,
			IMusicPlayer musicPlayer)
		{
			Helper.AssertIsNotNull(endGameButton, skipTutorialButton, resumeButton, retryButton, settingsButton);
			Helper.AssertIsNotNull(buttonsPanel, settingsPanel);
			Helper.AssertIsNotNull(soundPlayer, menuManager, settingsManager, musicPlayer);

			_menuManager = menuManager;

            _canvas = GetComponent<Canvas>();
            Assert.IsNotNull(_canvas);

			settingsPanel.Initialise(soundPlayer, menuManager, settingsManager, musicPlayer);

			// FELIX  Abstract to ButtonInitialiser :D
            endGameButton.Initialise(soundPlayer, _menuManager.QuitGame);
            skipTutorialButton.Initialise(soundPlayer, _menuManager.QuitGame);
            resumeButton.Initialise(soundPlayer, _menuManager.DismissMenu);
			retryButton.Initialise(soundPlayer, _menuManager.RetryLevel);
			settingsButton.Initialise(soundPlayer, _menuManager.ShowSettings);

            if (isTutorial)
            {
                Destroy(endGameButton.gameObject);
                Destroy(retryButton.gameObject);
			}
            else
            {
                Destroy(skipTutorialButton.gameObject);
            }

			_isVisible = new SettableBroadcastingProperty<bool>(initialValue: false);
			IsVisible = new BroadcastingProperty<bool>(_isVisible);

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
			_isVisible.Value = true;
			buttonsPanel.Show();
			settingsPanel.Hide();
		}

		public void HideMenu()
		{
			_canvas.enabled = false;
			_isVisible.Value = false;
		}

        public void ShowSettings()
        {
			buttonsPanel.Hide();
			settingsPanel.Show();
		}
    }
}
