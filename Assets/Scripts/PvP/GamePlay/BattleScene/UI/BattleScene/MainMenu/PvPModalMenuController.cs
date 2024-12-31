using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public class PvPModalMenuController : MonoBehaviour, IPvPModalMenu
    {
        private Canvas _canvas;

        public PvPMainMenuButtonsPanel buttonsPanel;
        public PvPInGameSettingsPanel settingsPanel;
        public PvPGameSpeedButton[] speedButtons;

        private IPvPSettableBroadcastingProperty<bool> _isVisible;
        public IPvPBroadcastingProperty<bool> IsVisible { get; private set; }

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            bool isTutorial,
            IPvPMainMenuManager menuManager,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(buttonsPanel, settingsPanel);
            PvPHelper.AssertIsNotNull(soundPlayer, menuManager, settingsManager);

            _canvas = GetComponent<Canvas>();
            Assert.IsNotNull(_canvas);

            buttonsPanel.Initialise(soundPlayer, isTutorial, menuManager);
            settingsPanel.Initialise(soundPlayer, menuManager, settingsManager);

            _isVisible = new PvPSettableBroadcastingProperty<bool>(initialValue: false);
            IsVisible = new PvPBroadcastingProperty<bool>(_isVisible);

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
