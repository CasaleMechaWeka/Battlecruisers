using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class StaticCameraTargetProviderTests
    {
        private IStaticCameraTargetProvider _targetProvider;
        private ICameraTarget _target1, _target2;
        private int _startedCount, _endedCount;

        [SetUp]
        public void TestSetup()
        {
            _targetProvider = new StaticCameraTargetProvider();
            _target1 = Substitute.For<ICameraTarget>();
            _target2 = Substitute.For<ICameraTarget>();

            _startedCount = 0;
            _targetProvider.UserInputStarted += (sender, e) => _startedCount++;

            _endedCount = 0;
            _targetProvider.UserInputEnded += (sender, e) => _endedCount++;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void SetTarget_NullToValid()
        {
            _targetProvider.SetTarget(_target1);

            Assert.AreSame(_target1, _targetProvider.Target);
            Assert.AreEqual(1, _startedCount);
        }

        [Test]
        public void SetTarget_ValidToValid()
        {
            // First target, triggers started event
            _targetProvider.SetTarget(_target1);

            Assert.AreSame(_target1, _targetProvider.Target);
            Assert.AreEqual(1, _startedCount);

            // Second target, does NOT trigger started event
            _targetProvider.SetTarget(_target2);

            Assert.AreSame(_target2, _targetProvider.Target);
            Assert.AreEqual(1, _startedCount);
        }

        [Test]
        public void SetTarget_ValidToNull()
        {
            // Null > valid
            _targetProvider.SetTarget(_target1);

            Assert.AreSame(_target1, _targetProvider.Target);
            Assert.AreEqual(1, _startedCount);

            // Valid > null
            _targetProvider.SetTarget(null);

            Assert.IsNull(_targetProvider.Target);
            Assert.AreEqual(1, _endedCount);
        }

        [Test]
        public void SetTarget_NullToNull()
        {
            _targetProvider.SetTarget(null);

            Assert.IsNull(_targetProvider.Target);
            Assert.AreEqual(0, _startedCount);
            Assert.AreEqual(0, _endedCount);
        }
    }
}