using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Use, test :)
    public class EdgeScrollingCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly IUpdater _updater;
        private readonly IEdgeScrollCalculator _scrollCalculator;
        private readonly ICamera _camera;
        private readonly ICameraCalculator _cameraCalculator;
        private readonly IEdgeDetector _edgeDetector;

        public override int Priority => 2;

        public EdgeScrollingCameraTargetProvider(
            IUpdater updater, 
            IEdgeScrollCalculator scrollCalculator, 
            ICamera camera, 
            ICameraCalculator cameraCalculator, 
            IEdgeDetector edgeDetector)
        {
            Helper.AssertIsNotNull(updater, scrollCalculator, camera, cameraCalculator, edgeDetector);

            _updater = updater;
            _scrollCalculator = scrollCalculator;
            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _edgeDetector = edgeDetector;

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
                = new CameraTarget(
                    new Vector3(targetCameraXPosition, _camera.Position.y, _camera.Position.z),
                    _camera.OrthographicSize);
        }

        private float FindTargetXPosition(bool moveRight)
        {
            float directionMultiplier = moveRight ? 1 : -1;
            float cameraDeltaX = _scrollCalculator.FindCameraPositionDeltaMagnituteInM();
            float targetXPosition = _camera.Position.x + (directionMultiplier * cameraDeltaX);

            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(_camera.OrthographicSize);
            return Mathf.Clamp(targetXPosition, validXPositions.Min, validXPositions.Max);
        }
    }
}