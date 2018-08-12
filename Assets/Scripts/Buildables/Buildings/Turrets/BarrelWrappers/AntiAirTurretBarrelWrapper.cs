using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class AntiAirTurretBarrelWrapper : LeadingDirectFireBarrelWrapper
    {
        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateAntiAirLimiter();
        }
    }
}
