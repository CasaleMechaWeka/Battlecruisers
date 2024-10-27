using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelFiringHelper : IBarrelFiringHelper
    {
        private readonly IBarrelController _barrelController;
        private readonly IAccuracyAdjuster _accuracyAdjuster;
        private readonly IFireIntervalManager _fireIntervalManager;
        private readonly IBarrelFirer _barrelFirer;

        private readonly bool _doDebug;

        public BarrelFiringHelper(
            IBarrelController barrelController,
            IAccuracyAdjuster accuracyAdjuster,
            IFireIntervalManager fireIntervalManager,
            IBarrelFirer barrelFirer,
            bool doDebug)
        {
            _doDebug = doDebug;
            Helper.AssertIsNotNull(barrelController, accuracyAdjuster, fireIntervalManager, barrelFirer);

            _barrelController = barrelController;
            _accuracyAdjuster = accuracyAdjuster;
            _fireIntervalManager = fireIntervalManager;
            _barrelFirer = barrelFirer;
        }

        public bool TryFire(BarrelAdjustmentResult barrelAdjustmentResult)
        {
            if (_doDebug)
                Debug.Log("TryFire");
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  _fireIntervalManager.ShouldFire: {_fireIntervalManager.ShouldFire.Value}");

            if (_fireIntervalManager.ShouldFire.Value)
            {
                if (_doDebug)
                    Debug.Log("ShouldFire");

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
                    if (_doDebug)
                        Debug.Log("IsOnTarget");
                    float fireAngleInDegrees
                        = _accuracyAdjuster.FindAngleInDegrees(
                            barrelAdjustmentResult.DesiredAngleInDegrees,
                            _barrelController.ProjectileSpawnerPosition,
                            barrelAdjustmentResult.PredictedTargetPosition,
                            _barrelController.IsSourceMirrored);

                    Fire(fireAngleInDegrees);
                    return true;
                }
                if (_doDebug)
                    Debug.Log("IsOffTarget");
            }

            return false;
        }

        private void Fire(float fireAngleInDegrees)
        {
            Debug.Log("Fire");
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  fireAngleInDegrees: {fireAngleInDegrees}");

            _barrelFirer.Fire(fireAngleInDegrees);
            _fireIntervalManager.OnFired();
        }
    }
}