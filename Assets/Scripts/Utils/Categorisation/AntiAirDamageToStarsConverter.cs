namespace BattleCruisers.Utils.Categorisation
{
    public class AntiAirDamageToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            22.5f,  // Anti-air turret
            25,     // Fighter
            33.75f, // Destroyer
            45      // Archon
        };

        public AntiAirDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
