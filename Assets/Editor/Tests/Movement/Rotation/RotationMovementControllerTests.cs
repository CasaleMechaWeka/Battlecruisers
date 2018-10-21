using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Utils.PlatformAbstractions;

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
            _rotateSpeedInDegreesPerS = 10;

            _movementController = new RotationMovementController(_rotationHelper, _transform, _time, _rotateSpeedInDegreesPerS);
        }

        [Test]
        public void SweetTest()
        {
        }
    }
}