namespace BattleCruisers.Data.Settings
{
    public enum Difficulty
    {
        Easy, Normal, Hard, Harder, Insane
    }

    public interface ISettingsManager
    {
        Difficulty AIDifficulty { get; set; }
		int ZoomSpeedLevel { get; set; }
        float ScrollSpeed { get; set; }

        void Save();
    }
}
