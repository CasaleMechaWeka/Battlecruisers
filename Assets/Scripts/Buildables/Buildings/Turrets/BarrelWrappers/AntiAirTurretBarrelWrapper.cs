using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class AntiAirTurretBarrelWrapper : LeadingDirectFireBarrelWrapper
    {
        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-180, 180);
        }
    }
}
