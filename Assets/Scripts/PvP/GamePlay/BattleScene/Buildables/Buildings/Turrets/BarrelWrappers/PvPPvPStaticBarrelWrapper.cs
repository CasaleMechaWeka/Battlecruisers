using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class PvPStaticBarrelWrapper : PvPBarrelWrapper
    {
        protected abstract float DesiredAngleInDegrees { get; }

        protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
        {
            return _factoryProvider.Turrets.AngleCalculatorFactory.CreateStaticAngleCalculator(DesiredAngleInDegrees);
        }

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return new DummyAngleLimiter();
        }
    }
}
