using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class DeferredBarrelFirerTests
    {
        private IBarrelFirer _deferredFirer, _coreFirer;
        private IConstantDeferrer _deferrer;

        [SetUp]
        public void TestSetup()
        {
            _coreFirer = Substitute.For<IBarrelFirer>();
            _deferrer = Substitute.For<IConstantDeferrer>();

            _deferredFirer = new DeferredBarrelFirer(_coreFirer, _deferrer);

            _deferrer.Defer(Arg.Invoke());
        }

        [Test]
        public void Fire()
        {
            float fireAngleInDegrees = 3.14f;

            _deferredFirer.Fire(fireAngleInDegrees);

            _deferrer.Received().Defer(Arg.Any<Action>());
            _coreFirer.Received().Fire(fireAngleInDegrees);
        }
    }
}