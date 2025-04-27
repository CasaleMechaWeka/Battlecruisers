using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Anti air turret
    /// Units:  Gunships
    /// </summary>
	public abstract class PvPLeadingDirectFireBarrelWrapper : PvPBarrelWrapper
    {
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return new LinearTargetPositionPredictor();
        }

        protected override IAngleCalculator CreateAngleCalculator(ProjectileStats projectileStats)
        {
            return new AngleCalculator();
        }
    }
}
