namespace BattleCruisers.Utils.Categorisation
{
    public class UnitMovementSpeedToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            0.1f,   // Archon
            2,      // Destroyer
            4,      // Frigate
            6,      // Attack boat
            8       // Fighter
        };

        public UnitMovementSpeedToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
