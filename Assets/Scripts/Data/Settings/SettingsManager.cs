using BattleCruisers.Data.Models;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly ISettingsModel _settings;
        private readonly IDataProvider _dataProvider;

        public event EventHandler SettingsSaved;

        public Difficulty AIDifficulty
        {
            get => _settings.AIDifficulty;
            set => _settings.AIDifficulty = value;
        }

        public int ZoomSpeedLevel
        {
            get => _settings.ZoomSpeedLevel;
            set => _settings.ZoomSpeedLevel = value;
        }

        public int ScrollSpeedLevel
        {
            get => _settings.ScrollSpeedLevel;
            set => _settings.ScrollSpeedLevel = value;
        }

        public float MusicVolume
        {
            get => _settings.MusicVolume;
            set => _settings.MusicVolume = value;
        }

        public float EffectVolume
        {
            get => _settings.EffectVolume;
            set => _settings.EffectVolume = value;
        }

        public bool MuteVoices
        {
            get => _settings.MuteVoices;
            set => _settings.MuteVoices = value;
        }

        public bool ShowInGameHints
        {
            get => _settings.ShowInGameHints;
            set => _settings.ShowInGameHints = value;
        }

        public SettingsManager(IDataProvider dataProvider)
        {
            Assert.IsNotNull(dataProvider);

            _settings = dataProvider.GameModel.Settings;
            _dataProvider = dataProvider;
        }

        public void Save()
        {
            _dataProvider.SaveGame();
            SettingsSaved?.Invoke(this, EventArgs.Empty);
        }
    }
}
