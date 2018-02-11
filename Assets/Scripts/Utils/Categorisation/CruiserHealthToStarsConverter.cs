namespace BattleCruisers.Utils.Categorisation
{
    public class CruiserHealthToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            2400,   // Raptor
            2800,   // Eagle
            3600,   // Bullshark
            3800,   // Rockjaw
            4000    // Longbow
        };

        public CruiserHealthToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
