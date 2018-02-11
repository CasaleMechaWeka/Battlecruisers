namespace BattleCruisers.Utils.Categorisation
{
    public class BuildableHealthToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            100,    // Bomber
            200,    // Gunship
            400,    // Rocket launcher
            1000    // Broadsides
        };

        public BuildableHealthToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
