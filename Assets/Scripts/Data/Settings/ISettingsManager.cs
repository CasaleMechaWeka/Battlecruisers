using System;

namespace BattleCruisers.Data.Settings
{
    public enum Difficulty
    {
        Easy, Normal, Hard, Harder
    }

    public interface ISettingsManager
    {
        event EventHandler SettingsSaved;

        Difficulty AIDifficulty { get; set; }
        int ZoomSpeedLevel { get; set; }
        int ScrollSpeedLevel { get; set; }

        float MusicVolume { get; set; }
        float EffectVolume { get; set; }

        bool ShowInGameHints { get; set; }

        void Save();
    }
}
