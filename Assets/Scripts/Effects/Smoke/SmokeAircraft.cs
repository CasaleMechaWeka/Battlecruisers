namespace BattleCruisers.Effects.Smoke
{
    public class SmokeAircraft : Smoke
    {
        protected override SmokeStatistics GetStatsForStrength(SmokeStrength strength)
        {
            switch (strength)
            {
                case SmokeStrength.Weak:
                    return StaticSmokeStats.Aircraft.Weak;

                case SmokeStrength.Normal:
                    return StaticSmokeStats.Aircraft.Normal;

                case SmokeStrength.Strong:
                    return StaticSmokeStats.Aircraft.Strong;

                default:
                    return null;
            }
        }
    }
}