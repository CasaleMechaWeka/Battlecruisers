namespace BattleCruisers.Data.Settings
{
    public enum Difficulty
    {
        Normal, Hard
    }

    public interface ISettingsManager
    {
        Difficulty AIDifficulty { get; set; }

        void Save();
    }
}