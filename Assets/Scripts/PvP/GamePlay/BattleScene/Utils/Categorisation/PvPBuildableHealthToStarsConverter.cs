namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public class PvPBuildableHealthToStarsConverter : PvPValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS =
        {
            1,      // Every building gets at least 1 star for health :)
            // 50, Shield
            // 100, Bomber
            // 120, Fighter
            // 150, Tesla coil, Stealth generator, Local booster
            // 160, Attack boat
            200,    // Gunship, AS turret, drone station, spy satellite, control tower
            // 220, AA turret
            400,    // Rocket launcher, air factory, naval factory
            // 440, SAM site, Mortar
            //500, Frigate
            600,    // Artillery, Railgun
            // 800, Destroyer
            1000    // Broadsides, Nuke, Ultralisk, Kamikaze signal
            // 4800, Archon battleship
        };

        public PvPBuildableHealthToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
