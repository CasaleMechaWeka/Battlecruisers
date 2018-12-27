using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Projectiles.Trackers
{
    public class TrackerTests
    {
        private Tracker _tracker;
        private ITrackable _trackable;
        private IBroadcastingFilter _trackerVisibilityFilter;
        private IMarker _marker;
        private ICamera _camera;
        private Vector3 _screenPosition;

        [SetUp]
        public void TestSetup()
        {
            _trackable = Substitute.For<ITrackable>();
            _trackerVisibilityFilter = Substitute.For<IBroadcastingFilter>();
            _marker = Substitute.For<IMarker>();
            _camera = Substitute.For<ICamera>();

            _trackable.Position.Returns(new Vector3(1, 2, 3));
            _screenPosition = new Vector3(-1, -2, -3);
            _camera.WorldToScreenPoint(_trackable.Position).Returns(_screenPosition);
            _trackerVisibilityFilter.IsMatch.Returns(false);

            _tracker = new Tracker(_trackable, _trackerVisibilityFilter, _marker, _camera);
        }

        [Test]
        public void Constructor()
        {
            ReceivedPositionUpdate();
            ReceivedVisibilityUpdate();
        }

        [Test]
        public void TrackablePositionChanged_UpdatesMarkerPosition()
        {
            ClearAllReceivedCalls();

            _trackable.PositionChanged += Raise.Event();

            ReceivedPositionUpdate();
        }

        [Test]
        public void TrackableDestroyed_DestroysTracker_UnsubscribesFromAllEvents()
        {
            ClearAllReceivedCalls();

            _trackable.Destroyed += Raise.Event();

            _marker.Received().Destroy();

            // PositionChanged event was unsubscribed
            _trackable.PositionChanged += Raise.Event();
            _camera.DidNotReceiveWithAnyArgs().WorldToScreenPoint(default(Vector3));

            // Destroyed event was unsubscribed
            _marker.ClearReceivedCalls();
            _trackable.Destroyed += Raise.Event();
            _marker.DidNotReceiveWithAnyArgs().Destroy();

            // PotentialMatchChange event was unsubscribed
            _trackerVisibilityFilter.PotentialMatchChange += Raise.Event();
            bool compilerBribe = _trackerVisibilityFilter.DidNotReceiveWithAnyArgs().IsMatch;
        }

        [Test]
        public void VisibilityFilter_PotentialMatchChanged_UpdatesMarkerVisibility()
        {
            ClearAllReceivedCalls();

            _trackerVisibilityFilter.PotentialMatchChange += Raise.Event();

            ReceivedVisibilityUpdate();
        }

        private void ReceivedPositionUpdate()
        {
            _camera.Received().WorldToScreenPoint(_trackable.Position);
            _marker.Received().OnScreenPostion = _screenPosition;
        }

        private void ReceivedVisibilityUpdate()
        {
            bool compilerBribe = _trackerVisibilityFilter.Received().IsMatch;
            _marker.Received().IsVisible = _trackerVisibilityFilter.IsMatch;
        }

        private void ClearAllReceivedCalls()
        {
            _camera.ClearReceivedCalls();
            _marker.ClearReceivedCalls();
            _trackerVisibilityFilter.ClearReceivedCalls();
        }
    }
}