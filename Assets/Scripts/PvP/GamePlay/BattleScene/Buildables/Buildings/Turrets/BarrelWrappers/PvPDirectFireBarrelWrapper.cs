using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPDirectFireBarrelWrapper : PvPBarrelWrapper
    {
        protected override IPvPAngleCalculator CreateAngleCalculator(IPvPProjectileStats projectileStats)
        {
            return _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleCalculator();
        }
    }
}
