namespace BattleCruisers.Utils.Categorisation
{
    public class AntiCruiserDamageToStarsConverter : ValueToStarsConverter
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

        public AntiCruiserDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
        {
        }
    }
}
