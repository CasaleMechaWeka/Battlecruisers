using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.BattleScene.MainMenu;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public class PvPModalMenuController : MonoBehaviour, IModalMenu
    {
        private Canvas _canvas;

        public PvPMainMenuButtonsPanel buttonsPanel;
        public PvPInGameSettingsPanel settingsPanel;
        public PvPGameSpeedButton[] speedButtons;

        private ISettableBroadcastingProperty<bool> _isVisible;
        public IBroadcastingProperty<bool> IsVisible { get; private set; }

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            bool isTutorial,
            IMainMenuManager menuManager,
            SettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(buttonsPanel, settingsPanel);
            PvPHelper.AssertIsNotNull(soundPlayer, menuManager, settingsManager);

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
            /*            for (int i = 0; i < 4; i++)
                        {
                            if (speedButtons[i].selectedFeedback.gameObject.activeInHierarchy)
                            {
                                lastClicked = i;
                            }
                        }*/
            //  speedButtons[0].TriggerClick();
        }

        public void HideMenu()
        {
            _canvas.enabled = false;
            _isVisible.Value = false;
            //    speedButtons[lastClicked].TriggerClick();
        }

        public void ShowSettings()
        {
            buttonsPanel.Hide();
            settingsPanel.Show();
        }
    }
}
