namespace BattleCruisers.Utils.Categorisation
{
    public class AntiShipDamageToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            24,     // Mortar
            50,     // Railgun
            87,     // Destroyer
            119     // Archon
        };

        public AntiShipDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
