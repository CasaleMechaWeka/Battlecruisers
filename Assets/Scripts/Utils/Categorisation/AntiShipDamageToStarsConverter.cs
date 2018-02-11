namespace BattleCruisers.Utils.Categorisation
{
    public class AntiShipDamageToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            24,     // Mortar
            40,     // Gunship
            80,     // Destroyer
            125     // Archon
        };

        public AntiShipDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
