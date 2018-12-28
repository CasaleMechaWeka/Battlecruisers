namespace BattleCruisers.Utils.Categorisation
{
    public class UnitMovementSpeedToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1.5f,   // Archon
            2,      // Destroyer
            // 3, Bomber
            4,      // Frigate, Gunship
            6,      // Attack boat
            8       // Fighter
        };

        public UnitMovementSpeedToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
