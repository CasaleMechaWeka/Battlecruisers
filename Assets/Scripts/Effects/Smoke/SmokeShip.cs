namespace BattleCruisers.Effects.Smoke
{
    public class SmokeShip : Smoke
    {
        protected override SmokeStatistics GetStatsForStrength(SmokeStrength strength)
        {
            switch (strength)
            {
                case SmokeStrength.Weak:
                    return StaticSmokeStats.Ship.Weak;

                case SmokeStrength.Normal:
                    return StaticSmokeStats.Ship.Normal;

                case SmokeStrength.Strong:
                    return StaticSmokeStats.Ship.Strong;

                default:
                    return null;
            }
        }
    }
}