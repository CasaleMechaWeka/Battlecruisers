using BattleCruisers.Movement.Rotation;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Tests.Movement.Rotation
{
    public class RotationMovementControllerTests
    {
        private IRotationMovementController _movementController;
        private IRotationHelper _rotationHelper;
        private ITransform _transform;
        private ITime _time;
        private float _rotateSpeedInDegreesPerS;

        [SetUp]
        public void TestSetup()
        {
            _rotationHelper = Substitute.For<IRotationHelper>();
            _transform = Substitute.For<ITransform>();
            _time = Substitute.For<ITime>();
            _rotateSpeedInDegreesPerS = 10;

            _movementController = new RotationMovementController(_rotationHelper, _transform, _time, _rotateSpeedInDegreesPerS);

            _transform.EulerAngles.Returns(new Vector3(0, 0, 12));
        }

        [Test]
        public void IsOnTarget_False()
        {
            float desiredAngleInDegrees = _transform.EulerAngles.z - RotationMovementController.ROTATION_EQUALITY_MARGIN_IN_DEGREES;
            Assert.IsFalse(_movementController.IsOnTarget(desiredAngleInDegrees));
        }

        [Test]
        public void IsOnTarget_True()
        {
            float desiredAngleInDegrees = _transform.EulerAngles.z - RotationMovementController.ROTATION_EQUALITY_MARGIN_IN_DEGREES + 0.001f;
            Assert.IsTrue(_movementController.IsOnTarget(desiredAngleInDegrees));
        }

        [Test]
        public void AdjustRotation_RotationIncrementIsSmallerThanDifference()
        {
            float desiredAngleInDegrees = 5;
            float directionMultiplier = -1;
            _rotationHelper.FindDirectionMultiplier(_transform.EulerAngles.z, desiredAngleInDegrees).Returns(directionMultiplier);
            _time.DeltaTime.Returns(0.2f);

            _movementController.AdjustRotation(desiredAngleInDegrees);

            float rotationIncrement = _time.DeltaTime * _rotateSpeedInDegreesPerS;
            Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement * directionMultiplier;
            _transform.Received().Rotate(rotationIncrementVector);
        }

        [Test]
        public void AdjustRotation_RotationIncrementIsLargerThanDifference()
        {
            float difference = -0.1f;
            float desiredAngleInDegrees = _transform.EulerAngles.z + difference;
            float directionMultiplier = -1;
            _rotationHelper.FindDirectionMultiplier(_transform.EulerAngles.z, desiredAngleInDegrees).Returns(directionMultiplier);
            _time.DeltaTime.Returns(0.2f);

            _movementController.AdjustRotation(desiredAngleInDegrees);

            // increment.z: -0.100004
            // difference:  -0.1
            _transform.Received().Rotate(Arg.Is<Vector3>(increment => Mathf.Abs(increment.z - difference) < 0.001f));
        }
    }
}