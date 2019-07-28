using BattleCruisers.Buildables;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetDetectors;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Tests.Targets.TargetDetectors
{
    public class ManualProximityTargetDetectorTests
    {
        private ManualProximityTargetDetector _detector;

        private ITransform _parentTransform;
        private float _detectionRange = 7;
        private IRangeCalculator _rangeCalculator;
        private ITarget _target1, _target2;
        private IList<ITarget> _enteredTargets, _exitedTargets;

        [SetUp]
        public void TestSetup()
        {
            _parentTransform = Substitute.For<ITransform>();
            _rangeCalculator = Substitute.For<IRangeCalculator>();

            _target1 = Substitute.For<ITarget>();
            _target2 = Substitute.For<ITarget>();

            List<ITarget> targets = new List<ITarget>()
            {
                _target1,
                _target2
            };
            IReadOnlyCollection<ITarget> readonlyTargets = targets.AsReadOnly();

            _detector = new ManualProximityTargetDetector(_parentTransform, readonlyTargets, _detectionRange, _rangeCalculator);

            _enteredTargets = new List<ITarget>();
            _detector.TargetEntered += (sender, e) => _enteredTargets.Add(e.Target);

            _exitedTargets = new List<ITarget>();
            _detector.TargetExited += (sender, e) => _exitedTargets.Add(e.Target);
        }

        [Test]
        public void Detect_NoInRangeTargets_DoesNothing()
        {
            MakeTargetLeaveRange(_target1);
            MakeTargetLeaveRange(_target2);

            _detector.Detect();

            Assert.AreEqual(0, _enteredTargets.Count);
        }

        [Test]
        public void Detect_NewInRangeTarget_EmitsEntered()
        {
            MakeTargetEnterRange(_target1);
            MakeTargetLeaveRange(_target2);

            _detector.Detect();

            Assert.AreEqual(1, _enteredTargets.Count);
            Assert.AreSame(_target1, _enteredTargets[0]);
        }

        [Test]
        public void Detect_NewInRangeTargets_EmitsEntered()
        {
            MakeTargetEnterRange(_target1);
            MakeTargetEnterRange(_target2);

            _detector.Detect();

            Assert.AreEqual(2, _enteredTargets.Count);
            Assert.IsTrue(_enteredTargets.Contains(_target1));
            Assert.IsTrue(_enteredTargets.Contains(_target2));
        }

        [Test]
        public void Detect_ExistingInRangeTarget_DoesNothing()
        {
            // Detect target 1 entering
            MakeTargetEnterRange(_target1);
            MakeTargetLeaveRange(_target2);
            _detector.Detect();
            Assert.AreSame(_target1, _enteredTargets[0]);

            // Second time does not re-detect
            _detector.Detect();
            Assert.AreEqual(1, _enteredTargets.Count);
        }

        [Test]
        public void Detect_TargetLeft_EmitsExited()
        {
            // Detect target 1 entering
            MakeTargetEnterRange(_target1);
            MakeTargetLeaveRange(_target2);
            _detector.Detect();

            // Detect target 1 exiting
            MakeTargetLeaveRange(_target1);

            _detector.Detect();

            Assert.AreEqual(1, _exitedTargets.Count);
            Assert.AreSame(_target1, _exitedTargets[0]);
        }

        [Test]
        public void Detect_TargetsLeft_EmitsExited()
        {
            // Detect targets entering
            MakeTargetEnterRange(_target1);
            MakeTargetEnterRange(_target2);
            _detector.Detect();

            // Detect targets exiting
            MakeTargetLeaveRange(_target1);
            MakeTargetLeaveRange(_target2);

            _detector.Detect();

            Assert.AreEqual(2, _exitedTargets.Count);
            Assert.IsTrue(_exitedTargets.Contains(_target1));
            Assert.IsTrue(_exitedTargets.Contains(_target2));
        }

        [Test]
        public void Detect_NewInRangeTarget_And_TargetLeft_EmitsStarted_And_EmitsExited()
        {
            // Detect target 1 entering
            MakeTargetEnterRange(_target1);
            MakeTargetLeaveRange(_target2);
            _detector.Detect();

            // Detect target 1 exiting and target 2 entering
            _enteredTargets.Clear();
            MakeTargetLeaveRange(_target1);
            MakeTargetEnterRange(_target2);

            _detector.Detect();

            Assert.AreEqual(1, _exitedTargets.Count);
            Assert.AreSame(_target1, _exitedTargets[0]);
            Assert.AreEqual(1, _enteredTargets.Count);
            Assert.AreSame(_target2, _enteredTargets[0]);

        }

        private void MakeTargetEnterRange(ITarget target)
        {
            _rangeCalculator.IsInRange(_parentTransform, target, _detectionRange).Returns(true);
        }

        private void MakeTargetLeaveRange(ITarget target)
        {
            _rangeCalculator.IsInRange(_parentTransform, target, _detectionRange).Returns(false);
        }
    }
}