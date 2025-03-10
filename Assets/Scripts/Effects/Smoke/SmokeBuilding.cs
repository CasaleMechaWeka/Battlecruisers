namespace BattleCruisers.Effects.Smoke
{
    public class SmokeBuilding : Smoke
    {
        protected override SmokeStatistics GetStatsForStrength(SmokeStrength strength)
        {
            switch (strength)
            {
                case SmokeStrength.Weak:
                    return StaticSmokeStats.Building.Weak;

                case SmokeStrength.Normal:
                    return StaticSmokeStats.Building.Normal;

                case SmokeStrength.Strong:
                    return StaticSmokeStats.Building.Strong;

                default:
                    return null;
            }
        }
    }
}