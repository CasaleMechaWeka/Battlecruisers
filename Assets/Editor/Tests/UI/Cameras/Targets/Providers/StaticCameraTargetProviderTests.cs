using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class StaticCameraTargetProviderTests
    {
        private IStaticCameraTargetProvider _targetProvider;
        private ICameraTarget _target;

        [SetUp]
        public void TestSetup()
        {
            _targetProvider = new StaticCameraTargetProvider(6);
            _target = Substitute.For<ICameraTarget>();
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNull(_targetProvider.Target);
            Assert.AreEqual(6, _targetProvider.Priority);
        }

        [Test]
        public void SetTarget()
        {
            _targetProvider.SetTarget(_target);
            Assert.AreSame(_target, _targetProvider.Target);
        }
    }
}