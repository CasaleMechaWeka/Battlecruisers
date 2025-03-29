namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class GunshipBarrelWrapper : LeadingDirectFireBarrelWrapper
    {
        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-180, 180);
        }
    }
}
