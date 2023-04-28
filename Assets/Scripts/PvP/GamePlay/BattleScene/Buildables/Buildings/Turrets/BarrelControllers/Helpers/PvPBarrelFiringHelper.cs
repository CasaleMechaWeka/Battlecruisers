using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class PvPBarrelFiringHelper : IPvPBarrelFiringHelper
    {
        private readonly IPvPBarrelController _barrelController;
        private readonly IPvPAccuracyAdjuster _accuracyAdjuster;
        private readonly IPvPFireIntervalManager _fireIntervalManager;
        private readonly IPvPBarrelFirer _barrelFirer;

        public PvPBarrelFiringHelper(
            IPvPBarrelController barrelController,
            IPvPAccuracyAdjuster accuracyAdjuster,
            IPvPFireIntervalManager fireIntervalManager,
            IPvPBarrelFirer barrelFirer)
        {
            PvPHelper.AssertIsNotNull(barrelController, accuracyAdjuster, fireIntervalManager, barrelFirer);

            _barrelController = barrelController;
            _accuracyAdjuster = accuracyAdjuster;
            _fireIntervalManager = fireIntervalManager;
            _barrelFirer = barrelFirer;
        }

        public bool TryFire(PvPBarrelAdjustmentResult barrelAdjustmentResult)
        {
            // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  _fireIntervalManager.ShouldFire: {_fireIntervalManager.ShouldFire.Value}");

            if (_fireIntervalManager.ShouldFire.Value)
            {
                // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  InBurst: {_barrelController.TurretStats.IsInBurst}  Current target: {_barrelController.CurrentTarget}  Can fire with no target: {_barrelController.CanFireWithoutTarget}  barrelAdjustmentResult.IsOnTarget: {barrelAdjustmentResult.IsOnTarget}");

                if (_barrelController.pvpTurretStats.IsInBurst
                    && (_barrelController.CurrentTarget != null
                        || _barrelController.CanFireWithoutTarget))
                {
                    // Burst fires happen even if we are no longer on target, so we may miss
                    // the target in this case.  Hence use the actual angle our turret barrel
                    // is at, instead of the perfect desired angle.
                    float fireAngleInDegrees
                        = _accuracyAdjuster.FindAngleInDegrees(
                            _barrelController.BarrelAngleInDegrees,
                            _barrelController.ProjectileSpawnerPosition,
                            barrelAdjustmentResult.PredictedTargetPosition,
                            _barrelController.IsSourceMirrored);

                    Fire(fireAngleInDegrees);
                    return true;
                }
                else if (barrelAdjustmentResult.IsOnTarget)
                {
                    float fireAngleInDegrees
                        = _accuracyAdjuster.FindAngleInDegrees(
                            barrelAdjustmentResult.DesiredAngleInDegrees,
                            _barrelController.ProjectileSpawnerPosition,
                            barrelAdjustmentResult.PredictedTargetPosition,
                            _barrelController.IsSourceMirrored);

                    Fire(fireAngleInDegrees);
                    return true;
                }
            }

            return false;
        }

        private void Fire(float fireAngleInDegrees)
        {
            // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  fireAngleInDegrees: {fireAngleInDegrees}");

            _barrelFirer.Fire(fireAngleInDegrees);
            _fireIntervalManager.OnFired();
        }
    }
}