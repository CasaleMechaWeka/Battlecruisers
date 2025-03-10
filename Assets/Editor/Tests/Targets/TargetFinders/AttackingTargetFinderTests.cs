using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetFinders
{
    public class AttackingTargetFinderTests
    {
        private ITargetFinder _finder;

        private IDamagable _parentDamagable;
        private ITargetFilter _targetFilter;
        private ITarget _target, _expectedTargetFound, _expectedTargetLost;
        private int _targetFoundCount, _targetLostCount;

        [SetUp]
        public void TestSetup()
        {
            _parentDamagable = Substitute.For<IDamagable>();
            _targetFilter = Substitute.For<ITargetFilter>();

            _finder = new AttackingTargetFinder(_parentDamagable, _targetFilter);

            _target = Substitute.For<ITarget>();
            _expectedTargetFound = null;
            _expectedTargetLost = null;

            _targetFoundCount = 0;
            _targetLostCount = 0;

            _finder.TargetFound += _finder_TargetFound;
            _finder.TargetLost += _finder_TargetLost;
        }

        private void _finder_TargetFound(object sender, TargetEventArgs e)
        {
            Assert.AreSame(_expectedTargetFound, e.Target);
            _targetFoundCount++;
        }

        private void _finder_TargetLost(object sender, TargetEventArgs e)
        {
            Assert.AreSame(_expectedTargetLost, e.Target);
            _targetLostCount++;
        }

        #region ParentDamaged
        [Test]
        public void ParentDamaged_BeforeStartFindingTargets_DoesNothing()
        {
            _finder = new AttackingTargetFinder(_parentDamagable, _targetFilter);
            _parentDamagable.Damaged += Raise.EventWith(new DamagedEventArgs(_target));
            Assert.AreEqual(0, _targetFoundCount);
        }

        [Test]
        public void ParentDamaged_AfterDisposed_DoesNothing()
        {
            _finder.DisposeManagedState();
            _parentDamagable.Damaged += Raise.EventWith(new DamagedEventArgs(_target));
            Assert.AreEqual(0, _targetFoundCount);
        }

        [Test]
        public void ParentDamaged_DamageSourceIsNull_DoesNothing()
        {
            _parentDamagable.Damaged += Raise.EventWith(new DamagedEventArgs(damageSource: null));
            Assert.AreEqual(0, _targetFoundCount);
        }

        [Test]
        public void ParentDamaged_DamageSourceIsDestroyed_DoesNothing()
        {
            _target.IsDestroyed.Returns(true);
            _parentDamagable.Damaged += Raise.EventWith(new DamagedEventArgs(_target));

            Assert.AreEqual(0, _targetFoundCount);
        }

        [Test]
        public void ParentDamaged_DamageSourceDoesNotMatchFilter_DoesNothing()
        {
            _target.IsDestroyed.Returns(false);
            _targetFilter.IsMatch(_target).Returns(false);
            _parentDamagable.Damaged += Raise.EventWith(new DamagedEventArgs(_target));

            Assert.AreEqual(0, _targetFoundCount);
        }

        [Test]
        public void ParentDamaged_EmitsTargetFound()
        {
            _target.IsDestroyed.Returns(false);
            _targetFilter.IsMatch(_target).Returns(true);

            _expectedTargetFound = _target;
            _parentDamagable.Damaged += Raise.EventWith(new DamagedEventArgs(_target));

            Assert.AreEqual(1, _targetFoundCount);
        }
        #endregion ParentDamaged

        [Test]
        public void TargetDestroyed_EmitsTargetLost()
        {
            // Find target
            _target.IsDestroyed.Returns(false);
            _targetFilter.IsMatch(_target).Returns(true);
            _expectedTargetFound = _target;
            _parentDamagable.Damaged += Raise.EventWith(new DamagedEventArgs(_target));

            // Lose target
            _expectedTargetLost = _target;
            _target.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target));
            Assert.AreEqual(1, _targetLostCount);
        }
    }
}