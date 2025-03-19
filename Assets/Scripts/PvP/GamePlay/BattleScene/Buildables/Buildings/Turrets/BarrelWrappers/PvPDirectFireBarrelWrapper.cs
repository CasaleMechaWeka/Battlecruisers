using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPDirectFireBarrelWrapper : PvPBarrelWrapper
    {
        protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
        {
            return new AngleCalculator();
        }
    }
}
