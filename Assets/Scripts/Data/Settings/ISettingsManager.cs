namespace BattleCruisers.Data.Settings
{
    public enum Difficulty
    {
        Sandbox, Normal, Hard
    }

    public interface ISettingsManager
    {
        Difficulty AIDifficulty { get; set; }
        int ZoomSpeed { get; set; }

        void Save();
    }
}
