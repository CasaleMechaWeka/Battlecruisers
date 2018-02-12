namespace BattleCruisers.Utils.Categorisation
{
    public class AntiCruiserDamageToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            41, // Rocket launcher
            50, // Artillery
            90, // Broadsides
            200 // Nuke
        };

        public AntiCruiserDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
