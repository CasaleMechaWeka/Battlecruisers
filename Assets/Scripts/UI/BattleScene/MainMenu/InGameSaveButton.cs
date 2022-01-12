using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Scenes.BattleScene;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public class InGameSaveButton : ElementWithClickSound
    {
        private IMainMenuManager _mainMenuManager;
        private ISettingsManager _settingsManager;
        private IBroadcastingProperty<float> _musicVolume, _effectVolume, _masterVolume, _alertVolume, _interfaceVolume, _ambientVolume;
        private IBroadcastingProperty<int> _zoomSpeedLevel, _scrollSpeedLevel;
        public BattleSceneGod god;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IMainMenuManager mainMenuManager,
            ISettingsManager settingsManager, 
            IBroadcastingProperty<float> musicVolume,
            IBroadcastingProperty<float> effectVolume,
            IBroadcastingProperty<int> zoomSpeedLevel,
            IBroadcastingProperty<int> scrollSpeedLevel,
            IBroadcastingProperty<float> masterVolume,
            IBroadcastingProperty<float> alertVolume,
            IBroadcastingProperty<float> interfaceVolume,
            IBroadcastingProperty<float> ambientVolume)
        {
            base.Initialise(soundPlayer, parent: mainMenuManager);

            Helper.AssertIsNotNull(mainMenuManager, settingsManager, musicVolume, effectVolume);

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
            _settingsManager.Save();
            
            UpdateEnabledStatus();

            god.UpdateCamera();
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
                || _ambientVolume.Value != _settingsManager.AmbientVolume;
        }
    }
}