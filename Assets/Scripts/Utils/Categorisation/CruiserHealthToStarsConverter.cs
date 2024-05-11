namespace BattleCruisers.Utils.Categorisation
{
    public class CruiserHealthToStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,      // 
                    // 1500, Raptor, Rickshaw
            2400,   // Eagle
                    // 3000, Trident
            3600,   // Rockjaw
                    // 3800, Bullshark
                    // 3900, Hammerhead
            4800,   //
                    // 4800, Longbow, Megalodon
            6000    // BlackRig, TasDevil, Yeti
        };

        public CruiserHealthToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
