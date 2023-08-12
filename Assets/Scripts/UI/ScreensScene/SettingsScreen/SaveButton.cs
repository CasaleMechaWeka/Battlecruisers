using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SaveButton : ElementWithClickSound
    {
        private IScreensSceneGod _screensSceneGod;
        private ISettingsManager _settingsManager;
        private IDifficultyDropdown _difficultyDropdown;
        private LanguageDropdown _languageDropdown;
        private ResolutionDropdown _resolutionDropdown;
        private IBroadcastingProperty<int> _zoomSpeedLevel, _scrollSpeedLevel;
        private IBroadcastingProperty<float> _musicVolume, _effectVolume, _masterVolume, _alertVolume, _interfaceVolume, _ambientVolume;
        private IBroadcastingProperty<bool> _showInGameHints, _showToolTips, _altDroneSounds, _showAds, _fullScreen, _VSync;
        private IHotkeysPanel _hotkeysPanel;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IDismissableEmitter parent,
            IScreensSceneGod screensSceneGod,
            ISettingsManager settingsManager, 
            IDifficultyDropdown difficultyDropdown,
            LanguageDropdown languageDropdown,
            ResolutionDropdown resolutionDropdown,
            IBroadcastingProperty<int> zoomSpeedLevel,
            IBroadcastingProperty<int> scrollSpeedLevel,
            IBroadcastingProperty<float> masterVolume,
            IBroadcastingProperty<float> effectVolume,
            IBroadcastingProperty<float> ambientVolume,
            IBroadcastingProperty<float> alertVolume,
            IBroadcastingProperty<float> interfaceVolume,
            IBroadcastingProperty<float> musicVolume,
            IBroadcastingProperty<bool> showInGameHints,
            IBroadcastingProperty<bool> showToolTips,
            IBroadcastingProperty<bool> altDroneSounds,
            IBroadcastingProperty<bool> showAds,
            IBroadcastingProperty<bool> fullScreen,
            IBroadcastingProperty<bool> VSync,
            IHotkeysPanel hotkeysPanel)
        {
            base.Initialise(soundPlayer, parent: parent);

            Helper.AssertIsNotNull(screensSceneGod, settingsManager, difficultyDropdown, zoomSpeedLevel, scrollSpeedLevel, musicVolume, effectVolume, showInGameHints, hotkeysPanel);

            _screensSceneGod = screensSceneGod;
            _settingsManager = settingsManager;
            _difficultyDropdown = difficultyDropdown;
            _languageDropdown = languageDropdown;
            _resolutionDropdown = resolutionDropdown;
            _zoomSpeedLevel = zoomSpeedLevel;
            _scrollSpeedLevel = scrollSpeedLevel;
            _masterVolume = masterVolume;
            _effectVolume = effectVolume;
            _ambientVolume = ambientVolume;
            _alertVolume = alertVolume;
            _interfaceVolume = interfaceVolume;
            _musicVolume = musicVolume;
            _showInGameHints = showInGameHints;
            _fullScreen = fullScreen;
            _VSync = VSync;
            _hotkeysPanel = hotkeysPanel;
            _showToolTips = showToolTips;
            _altDroneSounds = altDroneSounds;
            _showAds = showAds;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _difficultyDropdown.DifficultyChanged += (sender, e) => UpdateEnabledStatus();
            _languageDropdown.LanguageChanged += (sender, e) => UpdateEnabledStatus();
            _resolutionDropdown.ResolutionChanged += (sender, e) => UpdateEnabledStatus();
            _zoomSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _scrollSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _masterVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _effectVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _ambientVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _alertVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _interfaceVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _musicVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _showInGameHints.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _showToolTips.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _fullScreen.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _VSync.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _altDroneSounds.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _showAds.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _hotkeysPanel.IsDirty.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _settingsManager.Save();
            UpdateEnabledStatus();
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            Screen.SetResolution((int)_resolutionDropdown.Resolution.x, (int)_resolutionDropdown.Resolution.y - (_fullScreen.Value ? 0: (int)(_resolutionDropdown.Resolution.y*0.06)), _fullScreen.Value ? (FullScreenMode)1 : (FullScreenMode)3);
            QualitySettings.vSyncCount = _VSync.Value ? 1 : 0;
            
            Assert.IsTrue(ShouldBeEnabled());
            
            _hotkeysPanel.UpdateHokeysModel();

            _settingsManager.AIDifficulty = _difficultyDropdown.Difficulty;
            _settingsManager.Language = _languageDropdown.Language;
            _settingsManager.Resolution = _resolutionDropdown.Resolution;
            _settingsManager.ZoomSpeedLevel = _zoomSpeedLevel.Value;
            _settingsManager.ScrollSpeedLevel = _scrollSpeedLevel.Value;
            _settingsManager.MasterVolume = _masterVolume.Value;
            _settingsManager.EffectVolume = _effectVolume.Value;
            _settingsManager.AmbientVolume = _ambientVolume.Value;
            _settingsManager.AlertVolume = _alertVolume.Value;
            _settingsManager.InterfaceVolume = _interfaceVolume.Value;
            _settingsManager.MusicVolume = _musicVolume.Value;
            _settingsManager.ShowInGameHints = _showInGameHints.Value;
            _settingsManager.ShowToolTips = _showToolTips.Value;
            _settingsManager.AltDroneSounds = _altDroneSounds.Value;
            _settingsManager.ShowAds = _showAds.Value;
            _settingsManager.FullScreen = _fullScreen.Value;
            _settingsManager.VSync = _VSync.Value;
            _settingsManager.Save();

            UpdateEnabledStatus();

            //_screensSceneGod.GoToHomeScreen();
        }

        private void UpdateEnabledStatus()
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            //Debug.Log(_languageDropdown.Language + ", " + _settingsManager.Language);
            return
                _difficultyDropdown.Difficulty != _settingsManager.AIDifficulty
                || _languageDropdown.Language != _settingsManager.Language
                || _resolutionDropdown.Resolution != _settingsManager.Resolution
                || _zoomSpeedLevel.Value != _settingsManager.ZoomSpeedLevel
                || _scrollSpeedLevel.Value != _settingsManager.ScrollSpeedLevel
                || _masterVolume.Value != _settingsManager.MasterVolume
                || _effectVolume.Value != _settingsManager.EffectVolume
                || _ambientVolume.Value != _settingsManager.AmbientVolume
                || _alertVolume.Value != _settingsManager.AlertVolume
                || _interfaceVolume.Value != _settingsManager.InterfaceVolume
                || _musicVolume.Value != _settingsManager.MusicVolume
                || _showInGameHints.Value != _settingsManager.ShowInGameHints
                || _showToolTips.Value != _settingsManager.ShowToolTips
                || _altDroneSounds.Value != _settingsManager.AltDroneSounds
                 || _showAds.Value != _settingsManager.ShowAds
                || _VSync.Value != _settingsManager.VSync
                || _fullScreen.Value != _settingsManager.FullScreen
                || _hotkeysPanel.IsDirty.Value;
        }
    }
}