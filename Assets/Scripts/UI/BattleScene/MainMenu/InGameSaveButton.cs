using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    // FELIX  Avoid duplicate code with SaveButton :)
    public class InGameSaveButton : ElementWithClickSound
    {
        private IMainMenuManager _mainMenuManager;
        private ISettingsManager _settingsManager;
        private IBroadcastingProperty<float> _musicVolume, _effectVolume;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IMainMenuManager mainMenuManager,
            ISettingsManager settingsManager, 
            IBroadcastingProperty<float> musicVolume,
            IBroadcastingProperty<float> effectVolume)
        {
            base.Initialise(soundPlayer, parent: mainMenuManager);

            Helper.AssertIsNotNull(mainMenuManager, settingsManager, musicVolume, effectVolume);

            _mainMenuManager = mainMenuManager;
            _settingsManager = settingsManager;
            _musicVolume = musicVolume;
            _effectVolume = effectVolume;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _musicVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _effectVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();

            UpdateEnabledStatus();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            Assert.IsTrue(ShouldBeEnabled());

            _settingsManager.MusicVolume = _musicVolume.Value;
            _settingsManager.EffectVolume = _effectVolume.Value;
            _settingsManager.Save();

            UpdateEnabledStatus();

            _mainMenuManager.DismissMenu();
        }

        private void UpdateEnabledStatus()
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            return
                _musicVolume.Value != _settingsManager.MusicVolume
                || _effectVolume.Value != _settingsManager.EffectVolume;
        }
    }
}