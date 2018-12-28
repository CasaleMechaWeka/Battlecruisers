namespace BattleCruisers.Utils.Categorisation
{
    public class AntiAirDamageToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            // 9, SAM site
            // 13, Anti-air turret
            15,     // Fighter
            // 16, Frigate
            23,     // Destroyer
            27,     // Archon
            50      // No buildables achieve this :P
        };

        public AntiAirDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
