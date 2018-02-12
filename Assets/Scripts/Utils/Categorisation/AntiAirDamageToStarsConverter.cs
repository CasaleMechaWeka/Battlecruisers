namespace BattleCruisers.Utils.Categorisation
{
    public class AntiAirDamageToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            15,     // Fighter
            25,     // Destroyer
            32,     // Archon
            50      // No buildables achieve this :P
        };

        public AntiAirDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
