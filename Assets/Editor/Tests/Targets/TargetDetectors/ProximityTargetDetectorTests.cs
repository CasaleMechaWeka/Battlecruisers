using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Cruisers.Construction;
using UnityCommon.PlatformAbstractions;
using BattleCruisers.Buildables.Units;
using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Tests.Targets.TargetDetectors
{
    public class ProximityTargetDetectorTests
    {
        private ProximityTargetDetector _detector;

        private ITransform _parentTransform;
        private float _detectionRange = 7;
        private IUnit _target1, _target2;

        [SetUp]
        public void TestSetup()
        {
            _parentTransform = Substitute.For<ITransform>();
            _parentTransform.Position.Returns(new Vector3(0, 0, 0));

            _target1 = Substitute.For<IUnit>();
            _target2 = Substitute.For<IUnit>();

            List<IUnit> targets = new List<IUnit>()
            {
                _target1,
                _target2
            };
            IReadOnlyCollection<IUnit> readonlyTargets = targets.AsReadOnly();

            _detector = new ProximityTargetDetector(_parentTransform, readonlyTargets, _detectionRange);

            // FELIX Woooooooooooooooooow :D
            IReadOnlyCollection<IUnit> l1 = targets;
            IReadOnlyCollection<ITarget> l2 = l1;
        }

        [Test]
        public void Detect_NoInRangeTargets_DoesNothing()
        {
            MakeTargetLeaveRange(_target1);
        }

        [Test]
        public void Detect_NewInRangeTarget_EmitsEntered()
        {
        }

        [Test]
        public void Detect_NewInRangeTargets_EmitsEntered()
        {
        }

        [Test]
        public void Detect_ExistingInRangeTarget_DoesNothing()
        {
        }

        [Test]
        public void Detect_TargetLeft_EmitsExited()
        {
        }

        [Test]
        public void Detect_TargetsLeft_EmitsExited()
        {
        }

        [Test]
        public void Detect_NewInRangeTarget_And_TargetLeft_EmitsStarted_And_EmitsExited()
        {
        }

        private void MakeTargetEnterRange(IUnit target)
        {
            target.Transform.Position.Returns(new Vector3(_detectionRange, 0, 0));
        }

        private void MakeTargetLeaveRange(IUnit target)
        {
            target.Transform.Position.Returns(new Vector3(_detectionRange + 0.01f, 0, 0));
        }
    }
}