using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class PvPBarrelFirer : IPvPBarrelFirer
    {
        private readonly IPvPBarrelController _barrelController;
        private readonly IPvPAnimation _barrelFiringAnimation;
        private readonly IPvPParticleSystemGroup _muzzleFlash;

        public PvPBarrelFirer(
            IPvPBarrelController barrelController,
            IPvPAnimation barrelFiringAnimation,
            IPvPParticleSystemGroup muzzleFlash)
        {
            PvPHelper.AssertIsNotNull(barrelController, barrelFiringAnimation, muzzleFlash);

            _barrelController = barrelController;
            _barrelFiringAnimation = barrelFiringAnimation;
            _muzzleFlash = muzzleFlash;
        }

        public void Fire(float fireAngleInDegrees)
        {
            // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  fireAngleInDegrees: {fireAngleInDegrees}");

            _barrelController.Fire(fireAngleInDegrees);
            _barrelFiringAnimation.Play();
            _muzzleFlash.Play();
        }
    }
}
