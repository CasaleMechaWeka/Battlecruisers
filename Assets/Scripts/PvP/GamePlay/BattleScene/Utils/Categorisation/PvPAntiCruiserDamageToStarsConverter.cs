namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public class PvPAntiCruiserDamageToStarsConverter : PvPValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,
            //30, Artillery
            34, // Rocket launcher
            50, // Railgun
            //60, Deathstar
            90, // Broadsides
            200 // Nuke
        };

        public PvPAntiCruiserDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
