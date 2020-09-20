using BattleCruisers.Data.Settings;

namespace BattleCruisers.Data.Models
{
    public interface ISettingsModel
    {
        Difficulty AIDifficulty { get; set; }
        bool MuteMusic { get; set; }
        bool MuteVoices { get; set; }
        int ScrollSpeedLevel { get; set; }
        bool ShowInGameHints { get; set; }
        int ZoomSpeedLevel { get; set; }
    }
}