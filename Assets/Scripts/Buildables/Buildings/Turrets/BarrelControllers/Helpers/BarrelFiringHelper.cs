using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Effects;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelFiringHelper : IBarrelFiringHelper
    {
        private readonly IBarrelController _barrelController;
        private readonly IAccuracyAdjuster _accuracyAdjuster;
        private readonly IFireIntervalManager _fireIntervalManager;
        private readonly IAnimation _barrelFiringAnimation;
        private readonly IParticleSystemGroup _muzzleFlash;
        private readonly IConstantDeferrer _deferrer;

        public BarrelFiringHelper(
            IBarrelController barrelController, 
            IAccuracyAdjuster accuracyAdjuster, 
            IFireIntervalManager fireIntervalManager,
            IAnimation barrelFiringAnimation,
            IParticleSystemGroup muzzleFlash,
            IConstantDeferrer deferrer)
        {
            Helper.AssertIsNotNull(barrelController, accuracyAdjuster, fireIntervalManager, barrelFiringAnimation, muzzleFlash, deferrer);

            _barrelController = barrelController;
            _accuracyAdjuster = accuracyAdjuster;
            _fireIntervalManager = fireIntervalManager;
            _barrelFiringAnimation = barrelFiringAnimation;
            _muzzleFlash = muzzleFlash;
            _deferrer = deferrer;
        }

        public bool TryFire(BarrelAdjustmentResult barrelAdjustmentResult)
        {
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"_fireIntervalManager.ShouldFire: {_fireIntervalManager.ShouldFire}");

            if (_fireIntervalManager.ShouldFire)
            {
                if (_barrelController.TurretStats.IsInBurst
                    && (_barrelController.CurrentTarget != null
                        || _barrelController.CanFireWithoutTarget))
                {
                    // Burst fires happen even if we are no longer on target, so we may miss
                    // the target in this case.  Hence use the actual angle our turret barrel
                    // is at, instead of the perfect desired angle.
                    Fire(_barrelController.BarrelAngleInDegrees);
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
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"fireAngleInDegrees: {fireAngleInDegrees}");

            _deferrer.Defer(() => DelayedFire(fireAngleInDegrees));
            _fireIntervalManager.OnFired();
        }

        private void DelayedFire(float fireAngleInDegrees)
        {
            Logging.VerboseMethod(Tags.BARREL_CONTROLLER);

            _barrelController.Fire(fireAngleInDegrees);
            _barrelFiringAnimation.Play();
            _muzzleFlash.Play();
        }
    }
}
