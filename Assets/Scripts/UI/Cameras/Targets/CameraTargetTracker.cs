using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityCommon.Properties;

namespace BattleCruisers.UI.Cameras.Targets
{
    // FELIX  use, test
    public class CameraTargetTracker : ICameraTargetTracker
    {
        private readonly ICamera _camera;
        private readonly ICameraTarget _target;
        private readonly ICameraTargetEqualityCalculator _equalityCalculator;

        private readonly ISettableBroadcastingProperty<bool> _isOnTarget;
        public IBroadcastingProperty<bool> IsOnTarget { get; }

        public CameraTargetTracker(
            ICamera camera, 
            ICameraTarget target,
            ICameraTargetEqualityCalculator equalityCalculator)
        {
            Helper.AssertIsNotNull(camera, target, equalityCalculator);

            _camera = camera;
            _target = target;
            _equalityCalculator = equalityCalculator;

            _isOnTarget = new SettableBroadcastingProperty<bool>(initialValue: FindIfOnTarget());
            IsOnTarget = new BroadcastingProperty<bool>(_isOnTarget);

            _camera.PositionChanged += CameraChanged;
            _camera.OrthographicSizeChanged += CameraChanged;
        }

        private void CameraChanged(object sender, EventArgs e)
        {
            _isOnTarget.Value = FindIfOnTarget();
        }

        private bool FindIfOnTarget()
        {
            return _equalityCalculator.IsOnTarget(_target, _camera);
        }
    }
}