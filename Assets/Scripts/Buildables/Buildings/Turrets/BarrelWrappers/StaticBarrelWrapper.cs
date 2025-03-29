using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class StaticBarrelWrapper : BarrelWrapper
    {
        protected abstract float DesiredAngleInDegrees { get; }

        protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
        {
            return new StaticAngleCalculator(DesiredAngleInDegrees);
        }

        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-180, 180);
        }
    }
}
