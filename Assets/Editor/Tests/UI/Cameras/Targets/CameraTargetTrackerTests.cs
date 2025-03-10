using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras.Targets
{
    public class CameraTargetTrackerTests
    {
        private ICameraTargetTracker _tracker;
        private ICamera _camera;
        private ICameraTarget _target;
        private ICameraTargetEqualityCalculator _calculator;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _target = Substitute.For<ICameraTarget>();
            _calculator = Substitute.For<ICameraTargetEqualityCalculator>();

            _calculator.IsOnTarget(_target, _camera).Returns(true);

            _tracker
                = new CameraTargetTracker(
                    _camera,
                    _target,
                    _calculator);
        }

        [Test]
        public void Constructor()
        {
            _calculator.Received().IsOnTarget(_target, _camera);
            Assert.IsTrue(_tracker.IsOnTarget.Value);
        }

        [Test]
        public void CameraPositionChaneged()
        {
            _calculator.ClearReceivedCalls();
            _camera.PositionChanged += Raise.Event();
            _calculator.Received().IsOnTarget(_target, _camera);
        }

        [Test]
        public void CameraOrthographicSizeChanged()
        {
            _calculator.ClearReceivedCalls();
            _camera.OrthographicSizeChanged += Raise.Event();
            _calculator.Received().IsOnTarget(_target, _camera);
        }
    }
}