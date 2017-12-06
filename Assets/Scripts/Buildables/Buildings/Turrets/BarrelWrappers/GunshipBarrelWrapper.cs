using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class GunshipBarrelWrapper : LeadingDirectFireBarrelWrapper
    {
        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.AngleLimiterFactory.CreateDummyLimiter();
        }
    }
}
