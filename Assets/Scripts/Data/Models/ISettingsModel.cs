using BattleCruisers.Data.Settings;

namespace BattleCruisers.Data.Models
{
    public interface ISettingsModel
    {
        Difficulty AIDifficulty { get; set; }
        int ScrollSpeedLevel { get; set; }
        bool ShowInGameHints { get; set; }
        int ZoomSpeedLevel { get; set; }
        float MusicVolume { get; set; }
        float EffectVolume { get; set; }
    }
}