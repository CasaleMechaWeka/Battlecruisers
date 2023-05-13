namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public class PvPCruiserHealthToStarsConverter : PvPValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,   // Raptor
            2800,   // Eagle
            // 3000, Trident
            3600,   // Bullshark, Hammerhead
            4000,   // Rockjaw
            6000    // Longbow, Megalodon
        };

        public PvPCruiserHealthToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
