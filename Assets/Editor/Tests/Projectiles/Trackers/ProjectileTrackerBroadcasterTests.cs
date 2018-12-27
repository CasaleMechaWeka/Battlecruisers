using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Projectiles.Trackers
{
    public class ProjectileTrackerBroadcasterTests
    {
        private IBroadcastingFilter _broadcaster;
        private ICamera _camera;
        private const float ORTHOGRAPHIC_SIZE_THRESHOLD = 77;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _broadcaster = new ProjectileTrackerBroadcaster(_camera, ORTHOGRAPHIC_SIZE_THRESHOLD);
        }

        [Test]
        public void IsMatch_True()
        {
            _camera.OrthographicSize.Returns(ORTHOGRAPHIC_SIZE_THRESHOLD + 0.1f);
            Assert.IsTrue(_broadcaster.IsMatch);
        }

        [Test]
        public void IsMatch_False()
        {
            _camera.OrthographicSize.Returns(ORTHOGRAPHIC_SIZE_THRESHOLD - 0.1f);
            Assert.IsFalse(_broadcaster.IsMatch);
        }

        [Test]
        public void CameraOrthograpchiSizeChange_EmitsPotentialMatchChangeEvent()
        {
            bool wasEventCalled = false;
            _broadcaster.PotentialMatchChange += (sender, e) => wasEventCalled = true;

            _camera.OrthographicSizeChanged += Raise.Event();

            Assert.IsTrue(wasEventCalled);
        }
    }
}