namespace BattleCruisers.Utils.Categorisation
{
    public class AntiShipDamageToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            // 20, Anti-ship turret
            24,     // Mortar
            // 25, Attack boat
            // 26, Gunship
            50,     // Railgun
            // 73, Frigate
            87,     // Destroyer
            119     // Archon
        };

        public AntiShipDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
