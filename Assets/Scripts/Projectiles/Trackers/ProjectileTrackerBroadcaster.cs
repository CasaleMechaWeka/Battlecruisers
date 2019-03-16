using System;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Trackers
{
    public class ProjectileTrackerBroadcaster : IBroadcastingFilter
    {
        private readonly ICamera _camera;
        private readonly float _orthographicSizeThreshold;

        public event EventHandler PotentialMatchChange;

        public bool IsMatch => _camera.OrthographicSize >= _orthographicSizeThreshold;

        public ProjectileTrackerBroadcaster(ICamera camera, float orthographicSizeThreshold)
        {
            Assert.IsNotNull(camera);

            _camera = camera;
            _orthographicSizeThreshold = orthographicSizeThreshold;

            _camera.OrthographicSizeChanged += _camera_OrthographicSizeChanged;
        }

        private void _camera_OrthographicSizeChanged(object sender, EventArgs e)
        {
            PotentialMatchChange?.Invoke(this, EventArgs.Empty);
        }
    }
}