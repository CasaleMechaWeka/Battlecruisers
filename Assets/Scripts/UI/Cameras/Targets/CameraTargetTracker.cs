using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityCommon.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets
{
    // FELIX  use, test
    public class CameraTargetTracker : ICameraTargetTracker
    {
        private readonly ICamera _camera;
        private readonly ICameraTarget _target;
        private readonly ICameraTargetEqualityCalculator _equalityCalculator;
        private readonly float _positionEqualityMarginInM;
        private readonly float _orthographicSizeEqualityMargin;

        private readonly ISettableBroadcastingProperty<bool> _isOnTarget;
        public IBroadcastingProperty<bool> IsOnTarget { get; }

        public CameraTargetTracker(
            ICamera camera, 
            ICameraTarget target,
            ICameraTargetEqualityCalculator equalityCalculator,
            float positionEqualityMarginInM, 
            float orthographicSizeEqualityMargin)
        {
            Helper.AssertIsNotNull(camera, target, equalityCalculator);
            Assert.IsTrue(positionEqualityMarginInM > 0);
            Assert.IsTrue(orthographicSizeEqualityMargin > 0);

            _camera = camera;
            _target = target;
            _equalityCalculator = equalityCalculator;
            _positionEqualityMarginInM = positionEqualityMarginInM;
            _orthographicSizeEqualityMargin = orthographicSizeEqualityMargin;

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
            return _equalityCalculator.IsOnTarget(_target, _camera, _orthographicSizeEqualityMargin, _positionEqualityMarginInM);
        }
    }
}