using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Anti air turret
    /// Units:  Gunships
    /// </summary>
	public abstract class PvPLeadingDirectFireBarrelWrapper : PvPBarrelWrapper
    {
        protected override IPvPTargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateLinearPredictor();
        }

        protected override IPvPAngleCalculator CreateAngleCalculator(IPvPProjectileStats projectileStats)
        {
            return _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleCalculator();
        }
    }
}
