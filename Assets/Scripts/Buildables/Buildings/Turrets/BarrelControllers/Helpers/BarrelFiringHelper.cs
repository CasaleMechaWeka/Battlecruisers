using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelFiringHelper : IBarrelFiringHelper
    {
        private readonly IBarrelController _barrelController;
        private readonly IAccuracyAdjuster _accuracyAdjuster;
        private readonly IFireIntervalManager _fireIntervalManager;
        private readonly IBarrelFirer _barrelFirer;

        public BarrelFiringHelper(
            IBarrelController barrelController,
            IAccuracyAdjuster accuracyAdjuster,
            IFireIntervalManager fireIntervalManager,
            IBarrelFirer barrelFirer)
        {
            Helper.AssertIsNotNull(barrelController, accuracyAdjuster, fireIntervalManager, barrelFirer);

            _barrelController = barrelController;
            _accuracyAdjuster = accuracyAdjuster;
            _fireIntervalManager = fireIntervalManager;
            _barrelFirer = barrelFirer;
        }

        public bool TryFire(BarrelAdjustmentResult barrelAdjustmentResult)
        {
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  _fireIntervalManager.ShouldFire: {_fireIntervalManager.ShouldFire.Value}");

            if (_fireIntervalManager.ShouldFire.Value)
            {
                Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  InBurst: {_barrelController.TurretStats.IsInBurst}  Current target: {_barrelController.CurrentTarget}  Can fire with no target: {_barrelController.CanFireWithoutTarget}  barrelAdjustmentResult.IsOnTarget: {barrelAdjustmentResult.IsOnTarget}");

                if (_barrelController.TurretStats.IsInBurst
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
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  fireAngleInDegrees: {fireAngleInDegrees}");

            _barrelFirer.Fire(fireAngleInDegrees);
            _fireIntervalManager.OnFired();
        }
    }
}