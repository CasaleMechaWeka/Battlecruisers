namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class DeathstarBarrelWrapper : DirectFireBarrelWrapper
    {
        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-180, 180);
        }
    }
}
