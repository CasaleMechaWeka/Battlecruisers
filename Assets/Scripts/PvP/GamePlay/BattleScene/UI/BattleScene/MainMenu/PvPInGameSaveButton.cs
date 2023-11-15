using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras;
// using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public class PvPInGameSaveButton : PvPElementWithClickSound
    {
        private IPvPMainMenuManager _mainMenuManager;
        private ISettingsManager _settingsManager;
        private IPvPBroadcastingProperty<float> _musicVolume, _effectVolume, _masterVolume, _alertVolume, _interfaceVolume, _ambientVolume;
        private IPvPBroadcastingProperty<int> _zoomSpeedLevel, _scrollSpeedLevel;
        private IBroadcastingProperty<bool> _showToolTips;
        // public BattleSceneGod god;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            IPvPMainMenuManager mainMenuManager,
            ISettingsManager settingsManager,
            IPvPBroadcastingProperty<float> musicVolume,
            IPvPBroadcastingProperty<float> effectVolume,
            IPvPBroadcastingProperty<int> zoomSpeedLevel,
            IPvPBroadcastingProperty<int> scrollSpeedLevel,
            IPvPBroadcastingProperty<float> masterVolume,
            IPvPBroadcastingProperty<float> alertVolume,
            IPvPBroadcastingProperty<float> interfaceVolume,
            IPvPBroadcastingProperty<float> ambientVolume,
            IBroadcastingProperty<bool> showToolTips)
        {
            base.Initialise(soundPlayer, parent: mainMenuManager);

            PvPHelper.AssertIsNotNull(mainMenuManager, settingsManager, musicVolume, effectVolume);

            _mainMenuManager = mainMenuManager;
            _settingsManager = settingsManager;
            _musicVolume = musicVolume;
            _effectVolume = effectVolume;
            _masterVolume = masterVolume;
            _zoomSpeedLevel = zoomSpeedLevel;
            _scrollSpeedLevel = scrollSpeedLevel;
            _alertVolume = alertVolume;
            _interfaceVolume = interfaceVolume;
            _ambientVolume = ambientVolume;
            _showToolTips = showToolTips;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _musicVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _effectVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _zoomSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _scrollSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _masterVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _alertVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _interfaceVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _ambientVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _showToolTips.ValueChanged += (sender, e) => UpdateEnabledStatus();

            UpdateEnabledStatus();
            _settingsManager.Save();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            Assert.IsTrue(ShouldBeEnabled());

            _settingsManager.MusicVolume = _musicVolume.Value;
            _settingsManager.EffectVolume = _effectVolume.Value;
            _settingsManager.AlertVolume = _alertVolume.Value;
            _settingsManager.MasterVolume = _masterVolume.Value;
            _settingsManager.InterfaceVolume = _interfaceVolume.Value;
            _settingsManager.AmbientVolume = _ambientVolume.Value;
            _settingsManager.ZoomSpeedLevel = _zoomSpeedLevel.Value;
            _settingsManager.ScrollSpeedLevel = _scrollSpeedLevel.Value;
            _settingsManager.ShowToolTips = _showToolTips.Value;
            _settingsManager.Save();

            UpdateEnabledStatus();

            // god.UpdateCamera();
            //_mainMenuManager.DismissMenu();
        }

        private void UpdateEnabledStatus()
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            return
                _musicVolume.Value != _settingsManager.MusicVolume
                || _effectVolume.Value != _settingsManager.EffectVolume
                || _zoomSpeedLevel.Value != _settingsManager.ZoomSpeedLevel
                || _scrollSpeedLevel.Value != _settingsManager.ScrollSpeedLevel
                || _masterVolume.Value != _settingsManager.MasterVolume
                || _alertVolume.Value != _settingsManager.AlertVolume
                || _interfaceVolume.Value != _settingsManager.InterfaceVolume
                || _ambientVolume.Value != _settingsManager.AmbientVolume
                || _showToolTips.Value != _settingsManager.ShowToolTips;
        }
    }
}