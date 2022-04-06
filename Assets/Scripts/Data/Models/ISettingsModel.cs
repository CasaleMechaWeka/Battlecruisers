using BattleCruisers.Data.Settings;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    public interface ISettingsModel
    {
        Difficulty AIDifficulty { get; set; }
        int ScrollSpeedLevel { get; set; }
        bool ShowInGameHints { get; set; }
        bool ShowToolTips { get; set; }
        bool AltDroneSounds { get; set; }
        int ZoomSpeedLevel { get; set; }
        float MasterVolume { get; set; }
        float MusicVolume { get; set; }
        float EffectVolume { get; set; }
        float AlertVolume { get; set; }
        float InterfaceVolume { get; set; }
        float AmbientVolume { get; set; }
        string Language { get; set; }
        Vector2 Resolution { get; set; }
        
    }
}