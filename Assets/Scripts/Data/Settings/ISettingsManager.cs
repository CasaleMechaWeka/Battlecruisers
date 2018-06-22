namespace BattleCruisers.Data.Settings
{
    public enum Difficulty
    {
        Sandbox, Easy, Normal, Hard, Insane
    }

    public interface ISettingsManager
    {
        Difficulty AIDifficulty { get; set; }
		float ZoomSpeed { get; set; }
        float ScrollSpeed { get; set; }

        void Save();
    }
}
