using BattleCruisers.Data.Settings;
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

		public MainMenuButtonsPanel buttonsPanel;
		public InGameSettingsPanel settingsPanel;

		private ISettableBroadcastingProperty<bool> _isVisible;
		public IBroadcastingProperty<bool> IsVisible { get; private set; }

        public void Initialise(
			ISingleSoundPlayer soundPlayer, 
			bool isTutorial, 
			IMainMenuManager menuManager,
			ISettingsManager settingsManager)
		{
			Helper.AssertIsNotNull(buttonsPanel, settingsPanel);
			Helper.AssertIsNotNull(soundPlayer, menuManager, settingsManager);

			_menuManager = menuManager;

            _canvas = GetComponent<Canvas>();
            Assert.IsNotNull(_canvas);

			buttonsPanel.Initialise(soundPlayer, isTutorial, menuManager);
			settingsPanel.Initialise(soundPlayer, menuManager, settingsManager);

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
