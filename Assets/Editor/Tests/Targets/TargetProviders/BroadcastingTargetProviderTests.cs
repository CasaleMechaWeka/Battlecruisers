using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProviders;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetProviders
{
    public class DummyTargetProvider : BroadcastingTargetProvider
    {
        public void SetTarget(ITarget target)
        {
            Target = target;
        }

        public override void DisposeManagedState()
        {
            // empty
        }
    }

    public class BroadcastingTargetProviderTests
    {
        private DummyTargetProvider _targetProvider;
        private ITarget _target, _target2;
        private int _changeCount;

        [SetUp]
        public void SetuUp()
        {
            _changeCount = 0;

            _targetProvider = new DummyTargetProvider();

            _targetProvider.TargetChanged += (sender, e) => _changeCount++;

            _target = Substitute.For<ITarget>();
            _target2 = Substitute.For<ITarget>();
        }

        [Test]
        public void Null_ToDifferentTarget_EmitsEvent()
        {
            _targetProvider.SetTarget(_target);
            Assert.AreEqual(1, _changeCount);
        }

        [Test]
        public void Target_ToDifferentTarget_EmitsEvent()
        {
            _targetProvider.SetTarget(_target);
            Assert.AreEqual(1, _changeCount);

            _targetProvider.SetTarget(_target2);
            Assert.AreEqual(2, _changeCount);
        }

        [Test]
        public void SameTarget_Null_DoesNothing()
        {
            _targetProvider.SetTarget(target: null);
            Assert.AreEqual(0, _changeCount);
        }

        [Test]
        public void SameTarget_NonNull_DoesNothing()
        {
            _targetProvider.SetTarget(_target);
            Assert.AreEqual(1, _changeCount);

            _targetProvider.SetTarget(_target);
            Assert.AreEqual(1, _changeCount);
        }
    }
}
