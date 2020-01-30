using BattleCruisers.Effects;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    // FELIX  Interface, use
    // FELIX  Remove deferrer, instead create different BarrelFirer
    // FELIX  Test
    public class BarrelFirer : IBarrelFirer
    {
        private readonly IBarrelController _barrelController;
        private readonly IAnimation _barrelFiringAnimation;
        private readonly IParticleSystemGroup _muzzleFlash;
        private readonly IConstantDeferrer _deferrer;

        public BarrelFirer(
            IBarrelController barrelController,
            IAnimation barrelFiringAnimation,
            IParticleSystemGroup muzzleFlash,
            IConstantDeferrer deferrer)
        {
            Helper.AssertIsNotNull(barrelController, barrelFiringAnimation, muzzleFlash, deferrer);

            _barrelController = barrelController;
            _barrelFiringAnimation = barrelFiringAnimation;
            _muzzleFlash = muzzleFlash;
            _deferrer = deferrer;
        }

        public void Fire(float fireAngleInDegrees)
        {
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"fireAngleInDegrees: {fireAngleInDegrees}");
            _deferrer.Defer(() => DelayedFire(fireAngleInDegrees));
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
