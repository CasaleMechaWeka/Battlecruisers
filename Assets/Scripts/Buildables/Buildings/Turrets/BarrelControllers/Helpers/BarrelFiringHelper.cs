using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelFiringHelper : IBarrelFiringHelper
    {
        private readonly IBarrelController _barrelController;
        private readonly AccuracyAdjuster _accuracyAdjuster;
        private readonly IFireIntervalManager _fireIntervalManager;
        private readonly IBarrelFirer _barrelFirer;

        private readonly bool _doDebug;

        public BarrelFiringHelper(
            IBarrelController barrelController,
            AccuracyAdjuster accuracyAdjuster,
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
            //tolerance
            float angleTolerance = 15f;

            if (_doDebug)
                Debug.Log("MisFighter: TryFire");

            Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  _fireIntervalManager.ShouldFire: {_fireIntervalManager.ShouldFire.Value}");

            if (_fireIntervalManager.ShouldFire.Value)
            {
                if (_doDebug)
                    Debug.Log("MisFighter: ShouldFire");

                Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  InBurst: {_barrelController.TurretStats.IsInBurst}  Current target: {_barrelController.CurrentTarget}  Can fire with no target: {_barrelController.CanFireWithoutTarget}  barrelAdjustmentResult.IsOnTarget: {barrelAdjustmentResult.IsOnTarget}");

                if (_barrelController.TurretStats.IsInBurst
                    && (_barrelController.CurrentTarget != null || _barrelController.CanFireWithoutTarget))
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

                    Debug.Log($"MisFighter: Burst Mode - Fire Angle (Degrees): {fireAngleInDegrees}");

                    Fire(fireAngleInDegrees);
                    return true;
                }
                else if (barrelAdjustmentResult.IsOnTarget)
                {
                    if (_doDebug)
                        Debug.Log("MisFighter: IsOnTarget");

                    float fireAngleInDegrees = _accuracyAdjuster.FindAngleInDegrees(
                        barrelAdjustmentResult.DesiredAngleInDegrees,
                        _barrelController.ProjectileSpawnerPosition,
                        barrelAdjustmentResult.PredictedTargetPosition,
                        _barrelController.IsSourceMirrored);

                    //if is between the tolerance
                    if (Mathf.Abs(fireAngleInDegrees - barrelAdjustmentResult.DesiredAngleInDegrees) <= angleTolerance)
                    {
                        Debug.Log($"MisFighter: Within Tolerance - Fire Angle (Degrees): {fireAngleInDegrees}");
                        Debug.Log($"MisFighter: Desired Angle (Degrees): {barrelAdjustmentResult.DesiredAngleInDegrees}");
                        Fire(fireAngleInDegrees);
                        return true;
                    }
                    else
                    {
                        Debug.Log($"MisFighter: Outside Tolerance - Fire Angle (Degrees): {fireAngleInDegrees}, Desired: {barrelAdjustmentResult.DesiredAngleInDegrees}");
                    }
                }
                else if (!barrelAdjustmentResult.IsOnTarget)
                {
                    if (_doDebug)
                        Debug.Log("MisFighter: IsOffTarget - Desired Angle Not Met.");
                }
            }

            return false;
        }


        private void Fire(float fireAngleInDegrees)
        {
            Debug.Log("MisFighter: Fire");
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  fireAngleInDegrees: {fireAngleInDegrees}");

            _barrelFirer.Fire(fireAngleInDegrees);
            _fireIntervalManager.OnFired();
        }
    }
}