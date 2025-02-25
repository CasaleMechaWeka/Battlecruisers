using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public class PvPEdgeScrollingCameraTargetProvider : PvPUserInputCameraTargetProvider
    {
        private readonly IUpdater _updater;
        private readonly IPvPEdgeScrollCalculator _scrollCalculator;
        private readonly ICamera _camera;
        private readonly IPvPCameraCalculator _cameraCalculator;
        private readonly IPvPEdgeDetector _edgeDetector;
        private readonly IClamper _cameraXPositionClamper;

        public override int Priority => 2;

        public PvPEdgeScrollingCameraTargetProvider(
            IUpdater updater,
            IPvPEdgeScrollCalculator scrollCalculator,
            ICamera camera,
            IPvPCameraCalculator cameraCalculator,
            IPvPEdgeDetector edgeDetector,
            IClamper cameraXPositionClamper)
        {
            PvPHelper.AssertIsNotNull(updater, scrollCalculator, camera, cameraCalculator, edgeDetector, cameraXPositionClamper);

            _updater = updater;
            _scrollCalculator = scrollCalculator;
            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _edgeDetector = edgeDetector;
            _cameraXPositionClamper = cameraXPositionClamper;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (!_edgeDetector.IsCursorAtLeftEdge()
                && !_edgeDetector.IsCursorAtRightEdge())
            {
                UserInputEnd();
                return;
            }

            bool moveRight = _edgeDetector.IsCursorAtRightEdge();
            float targetCameraXPosition = FindTargetXPosition(moveRight);

            Target
                = new PvPCameraTarget(
                    new Vector3(targetCameraXPosition, _camera.Position.y, _camera.Position.z),
                    _camera.OrthographicSize);
        }

        private float FindTargetXPosition(bool moveRight)
        {
            float directionMultiplier = moveRight ? 1 : -1;
            float cameraDeltaX = _scrollCalculator.FindCameraPositionDeltaMagnituteInM();
            float targetXPosition = _camera.Position.x + (directionMultiplier * cameraDeltaX);

            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(_camera.OrthographicSize);
            return _cameraXPositionClamper.Clamp(targetXPosition, validXPositions);
        }
    }
}