using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class PvPStaticBarrelWrapper : PvPBarrelWrapper
    {
        protected abstract float DesiredAngleInDegrees { get; }

        protected override IPvPAngleCalculator CreateAngleCalculator(IPvPProjectileStats projectileStats)
        {
            return _factoryProvider.Turrets.AngleCalculatorFactory.CreateStaticAngleCalculator(DesiredAngleInDegrees);
        }

        protected override AngleLimiters.IPvPAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateDummyLimiter();
        }
    }
}
