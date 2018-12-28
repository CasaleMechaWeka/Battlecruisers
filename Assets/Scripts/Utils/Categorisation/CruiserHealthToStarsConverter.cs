namespace BattleCruisers.Utils.Categorisation
{
    public class CruiserHealthToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1200,   // Raptor
            2800,   // Eagle
            // 3000, Trident
            3600,   // Bullshark, Hammerhead
            3800,   // Rockjaw
            4000    // Longbow, Megalodon
        };

        public CruiserHealthToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
