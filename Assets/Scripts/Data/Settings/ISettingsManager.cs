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

        void Save();
    }
}
