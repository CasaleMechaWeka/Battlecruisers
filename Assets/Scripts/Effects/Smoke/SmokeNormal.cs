namespace BattleCruisers.Effects.Smoke
{
    public class SmokeNormal : Smoke
    {
        protected override SmokeStats GetStatsForStrength(SmokeStrength strength)
        {
            switch (strength)
            {
                case SmokeStrength.Weak:
                    return StaticSmokeStats.Normal.WeakSmoke;

                case SmokeStrength.Normal:
                    return StaticSmokeStats.Normal.NormalSmoke;

                case SmokeStrength.Strong:
                    return StaticSmokeStats.Normal.StrongSmoke;

                default:
                    return null;
            }
        }
    }
}