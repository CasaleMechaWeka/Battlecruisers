namespace BattleCruisers.Data.Settings
{
    public enum Difficulty
    {
        Easy, Normal, Hard, Harder
    }

    public interface ISettingsManager
    {
        Difficulty AIDifficulty { get; set; }
        int ZoomSpeedLevel { get; set; }
        int ScrollSpeedLevel { get; set; }

        float MusicVolume { get; set; }
        float EffectVolume { get; set; }

        // FELIX  Replace with volume :)
        bool MuteVoices { get; set; }
        bool ShowInGameHints { get; set; }

        void Save();
    }
}
