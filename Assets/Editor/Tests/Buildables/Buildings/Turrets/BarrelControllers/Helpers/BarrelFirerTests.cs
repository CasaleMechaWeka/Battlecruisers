using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Effects;
using BattleCruisers.Effects.ParticleSystems;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelFirerTests
    {
        private IBarrelFirer _barrelFirer;
        private IBarrelController _barrelController;
        private IAnimation _barrelFiringAnimation;
        private IParticleSystemGroup _muzzleFlash;

        [SetUp]
        public void TestSetup()
        {
            _barrelController = Substitute.For<IBarrelController>();
            _barrelFiringAnimation = Substitute.For<IAnimation>();
            _muzzleFlash = Substitute.For<IParticleSystemGroup>();

            _barrelFirer = new BarrelFirer(_barrelController, _barrelFiringAnimation, _muzzleFlash);
        }

        [Test]
        public void Fire()
        {
            float fireAngleInDegrees = 12.7f;

            _barrelFirer.Fire(fireAngleInDegrees);

            _barrelController.Received().Fire(fireAngleInDegrees);
            _barrelFiringAnimation.Received().Play();
            _muzzleFlash.Received().Play();
        }
    }
}