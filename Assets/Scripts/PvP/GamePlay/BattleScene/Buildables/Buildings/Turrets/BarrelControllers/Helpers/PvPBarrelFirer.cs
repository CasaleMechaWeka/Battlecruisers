using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Effects;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class PvPBarrelFirer : IBarrelFirer
    {
        private readonly IBarrelController _barrelController;
        private readonly IAnimation _barrelFiringAnimation;
        private readonly IParticleSystemGroup _muzzleFlash;

        public PvPBarrelFirer(
            IBarrelController barrelController,
            IAnimation barrelFiringAnimation,
            IParticleSystemGroup muzzleFlash)
        {
            PvPHelper.AssertIsNotNull(barrelController, muzzleFlash);

            _barrelController = barrelController;
            _barrelFiringAnimation = barrelFiringAnimation;
            _muzzleFlash = muzzleFlash;
        }

        public void Fire(float fireAngleInDegrees)
        {
            // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  fireAngleInDegrees: {fireAngleInDegrees}");

            _barrelController.Fire(fireAngleInDegrees);

            if (_barrelFiringAnimation != null)
                _barrelFiringAnimation.Play();

            _muzzleFlash.Play();
        }
    }
}
