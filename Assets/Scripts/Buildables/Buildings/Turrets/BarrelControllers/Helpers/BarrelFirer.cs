using BattleCruisers.Effects;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelFirer : IBarrelFirer
    {
        private readonly IBarrelController _barrelController;
        private readonly IAnimation _barrelFiringAnimation;
        private readonly IParticleSystemGroup _muzzleFlash;

        public BarrelFirer(
            IBarrelController barrelController,
            IAnimation barrelFiringAnimation,
            IParticleSystemGroup muzzleFlash)
        {
            Helper.AssertIsNotNull(barrelController, barrelFiringAnimation, muzzleFlash);

            _barrelController = barrelController;
            _barrelFiringAnimation = barrelFiringAnimation;
            _muzzleFlash = muzzleFlash;
        }

        public void Fire(float fireAngleInDegrees)
        {
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"fireAngleInDegrees: {fireAngleInDegrees}");

            _barrelController.Fire(fireAngleInDegrees);
            _barrelFiringAnimation.Play();
            _muzzleFlash.Play();
        }
    }
}
