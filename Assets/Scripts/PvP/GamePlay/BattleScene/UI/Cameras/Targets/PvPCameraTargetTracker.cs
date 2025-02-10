using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using BattleCruisers.Utils.Properties;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public class PvPCameraTargetTracker : IPvPCameraTargetTracker
    {
        private readonly IPvPCamera _camera;
        private readonly IPvPCameraTarget _target;
        private readonly IPvPCameraTargetEqualityCalculator _equalityCalculator;

        private readonly ISettableBroadcastingProperty<bool> _isOnTarget;
        public IBroadcastingProperty<bool> IsOnTarget { get; }

        public PvPCameraTargetTracker(
            IPvPCamera camera,
            IPvPCameraTarget target,
            IPvPCameraTargetEqualityCalculator equalityCalculator)
        {
            PvPHelper.AssertIsNotNull(camera, target, equalityCalculator);

            _camera = camera;
            _target = target;
            _equalityCalculator = equalityCalculator;

            _isOnTarget = new PvPSettableBroadcastingProperty<bool>(initialValue: FindIfOnTarget());
            IsOnTarget = new PvPBroadcastingProperty<bool>(_isOnTarget);

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