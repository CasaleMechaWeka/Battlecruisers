namespace BattleCruisers.Utils.Categorisation
{
    public class AntiAirDamageToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
                    // AirTurret:   7 DPS
                    // Frigate:     7.7 DPS
            10,     
                    // SAMSite:     11.5 DPS
                    // Destroyer:   13.3 DPS
            23,     
                    // Fighter:     24 DPS
            27,     // Archon
            50      // No buildables achieve this :P
        };

        public AntiAirDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
