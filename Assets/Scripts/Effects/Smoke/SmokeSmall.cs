namespace BattleCruisers.Effects.Smoke
{
    public class SmokeSmall : Smoke
    {
        protected override SmokeStats GetStatsForStrength(SmokeStrength strength)
        {
            switch (strength)
            {
                case SmokeStrength.Weak:
                    return StaticSmokeStats.Small.WeakSmoke;

                case SmokeStrength.Normal:
                    return StaticSmokeStats.Small.NormalSmoke;

                case SmokeStrength.Strong:
                    return StaticSmokeStats.Small.StrongSmoke;

                default:
                    return null;
            }
        }
    }
}