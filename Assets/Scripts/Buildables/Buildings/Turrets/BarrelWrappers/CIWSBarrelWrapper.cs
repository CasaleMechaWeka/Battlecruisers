using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class CIWSBarrelWrapper : LeadingDirectFireBarrelWrapper
    {
        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-30, 180);
        }
    }
}
