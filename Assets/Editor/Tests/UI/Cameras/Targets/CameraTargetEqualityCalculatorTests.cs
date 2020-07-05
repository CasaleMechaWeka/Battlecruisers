using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets
{
    public class CameraTargetEqualityCalculatorTests
    {
        private ICameraTargetEqualityCalculator _calculator;
        private float _positionEqualityMarginInM, _orthographicSizeEqualityMargin;
        private ICamera _camera;
        private ICameraTarget _target;

        [SetUp]
        public void TestSetup()
        {
            _positionEqualityMarginInM = 1;
            _orthographicSizeEqualityMargin = 5;

            _calculator = new CameraTargetEqualityCalculator(_positionEqualityMarginInM, _orthographicSizeEqualityMargin);

            _camera = Substitute.For<ICamera>();

            _target = Substitute.For<ICameraTarget>();
            _target.Position.Returns(Vector3.zero);
            _target.OrthographicSize.Returns(0);
        }

        [Test]
        public void OutsidePosition_OutsideOrthographicSize()
        {
            _camera.Position.Returns(new Vector3(_positionEqualityMarginInM, 0, 0));
            _camera.OrthographicSize.Returns(_orthographicSizeEqualityMargin);

            Assert.IsFalse(_calculator.IsOnTarget(_target, _camera));
        }

        [Test]
        public void WithinPosition_OutsideOrthographicSize()
        {
            _camera.Position.Returns(new Vector3(_positionEqualityMarginInM - 0.1f, 0, 0));
            _camera.OrthographicSize.Returns(_orthographicSizeEqualityMargin);

            Assert.IsFalse(_calculator.IsOnTarget(_target, _camera));
        }

        [Test]
        public void OutsidePosition_WithinOrthographicSize()
        {
            _camera.Position.Returns(new Vector3(_positionEqualityMarginInM, 0, 0));
            _camera.OrthographicSize.Returns(_orthographicSizeEqualityMargin - 0.1f);

            Assert.IsFalse(_calculator.IsOnTarget(_target, _camera));
        }

        [Test]
        public void WithinPosition_WithinOrthographicSize()
        {
            _camera.Position.Returns(new Vector3(_positionEqualityMarginInM - 0.1f, 0, 0));
            _camera.OrthographicSize.Returns(_orthographicSizeEqualityMargin - 0.1f);

            Assert.IsTrue(_calculator.IsOnTarget(_target, _camera));
        }
    }
}