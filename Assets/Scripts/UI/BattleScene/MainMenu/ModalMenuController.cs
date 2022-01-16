using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.BattleScene.GameSpeed;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public class ModalMenuController : MonoBehaviour, IModalMenu
	{
		private Canvas _canvas;

		public MainMenuButtonsPanel buttonsPanel;
		public InGameSettingsPanel settingsPanel;
		public GameSpeedButton playButton;
		public GameSpeedButton pauseButton;

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

            _canvas = GetComponent<Canvas>();
            Assert.IsNotNull(_canvas);

			buttonsPanel.Initialise(soundPlayer, isTutorial, menuManager);
			settingsPanel.Initialise(soundPlayer, menuManager, settingsManager);

			_isVisible = new SettableBroadcastingProperty<bool>(initialValue: false);
			IsVisible = new BroadcastingProperty<bool>(_isVisible);

			HideMenu();
		}

		public void ShowMenu()
		{
			_canvas.enabled = true;
			_isVisible.Value = true;
			buttonsPanel.Show();
			settingsPanel.Hide();
			pauseButton.TriggerClick();
		}

		public void HideMenu()
		{
			_canvas.enabled = false;
			_isVisible.Value = false;
			playButton.TriggerClick();
		}

        public void ShowSettings()
        {
			buttonsPanel.Hide();
			settingsPanel.Show();
		}
    }
}
