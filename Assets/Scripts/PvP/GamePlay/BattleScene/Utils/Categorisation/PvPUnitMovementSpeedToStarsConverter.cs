namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public class PvPUnitMovementSpeedToStarsConverter : PvPValueToStarsConverter
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

        public PvPUnitMovementSpeedToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
