using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Trackers
{
    public class TrackerFactory : ITrackerFactory
    {
        private readonly IMarkerFactory _markerFactory;
        private readonly ICamera _camera;

        // Min:  5, Max:  @16:9  24.75  @4:3  33
        private const float ORTHOGRAPHIC_THRESHOLD_SIZE = 15;

        public TrackerFactory(IMarkerFactory markerFactory, ICamera camera)
        {
            Helper.AssertIsNotNull(markerFactory, camera);

            _markerFactory = markerFactory;
            _camera = camera;
        }

        public Tracker CreateTracker(ITrackable trackable)
        {
            Assert.IsNotNull(trackable);

            return
                new Tracker(
                    trackable,
                    new ProjectileTrackerBroadcaster(_camera, ORTHOGRAPHIC_THRESHOLD_SIZE),
                    _markerFactory.CreateMarker(),
                    _camera);
        }
    }
}