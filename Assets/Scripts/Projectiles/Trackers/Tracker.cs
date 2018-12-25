using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Projectiles.Trackers
{
    /// <summary>
    /// Makes a marker track a trackable when visibility is enabled.
    /// 
    /// Eg:  When the user zooms out past a certain level, provide
    /// a marker for projectiles so they are still easily visible.
    /// </summary>
    // FELIX  Test :)
    public class Tracker
    {
        private readonly ITrackable _trackable;
        private readonly IBroadcastingFilter _trackerVisibilityFilter;
        private readonly IMarker _marker;
        private readonly ICamera _camera;

        public Tracker(
            ITrackable trackable, 
            IBroadcastingFilter trackerVisibilityFilter, 
            IMarker marker,
            ICamera camera)
        {
            Helper.AssertIsNotNull(trackable, trackerVisibilityFilter, marker, camera);

            _trackable = trackable;
            _trackerVisibilityFilter = trackerVisibilityFilter;
            _marker = marker;
            _camera = camera;

            UpdateMarkerVisibility();

            _trackable.PositionChanged += _trackable_PositionChanged;
            _trackable.Destroyed += _trackable_Destroyed;
            _trackerVisibilityFilter.PotentialMatchChange += _trackerVisibilityFilter_PotentialMatchChange;
        }

        private void _trackable_PositionChanged(object sender, EventArgs e)
        {
            _marker.OnScreenPostion = _camera.WorldToScreenPoint(_trackable.Position);
        }

        private void _trackable_Destroyed(object sender, EventArgs e)
        {
            _trackable.PositionChanged -= _trackable_PositionChanged;
            _trackable.Destroyed -= _trackable_Destroyed;
        }

        private void _trackerVisibilityFilter_PotentialMatchChange(object sender, EventArgs e)
        {
            UpdateMarkerVisibility();
        }

        private void UpdateMarkerVisibility()
        {
            _marker.IsVisible = _trackerVisibilityFilter.IsMatch;
        }
    }
}