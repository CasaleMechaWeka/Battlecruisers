namespace BattleCruisers.Effects
{
    public class SmokeBig : Smoke
    {
        protected override SmokeStats GetStatsForStrength(SmokeStrength strength)
        {
            switch (strength)
            {
                case SmokeStrength.Weak:
                    return StaticSmokeStats.Big.WeakSmoke;

                case SmokeStrength.Normal:
                    return StaticSmokeStats.Big.NormalSmoke;

                case SmokeStrength.Strong:
                    return StaticSmokeStats.Big.StrongSmoke;

                default:
                    return null;
            }
        }
    }
}