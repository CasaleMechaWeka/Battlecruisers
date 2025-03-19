using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
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

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return new DummyAngleLimiter();
        }
    }
}
