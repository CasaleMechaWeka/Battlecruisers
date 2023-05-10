using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Clamping;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public class PvPSwipeCameraTargetProvider : PvPUserInputCameraTargetProvider
    {
        private readonly IPvPDragTracker _dragTracker;
        private readonly IPvPScrollCalculator _scrollCalculator;
        private readonly IPvPZoomCalculator _zoomCalculator;
        private readonly IPvPCamera _camera;
        private readonly IPvPCameraCalculator _cameraCalculator;
        private readonly IPvPDirectionalZoom _directionalZoom;
        private readonly IPvPScrollRecogniser _scrollRecogniser;
        private readonly IPvPClamper _cameraXPositionClamper;

        public override int Priority => 3;

        public PvPSwipeCameraTargetProvider(
            IPvPDragTracker dragTracker,
            IPvPScrollCalculator scrollCalculator,
            IPvPZoomCalculator zoomCalculator,
            IPvPCamera camera,
            IPvPCameraCalculator cameraCalculator,
            IPvPDirectionalZoom directionalZoom,
            IPvPScrollRecogniser scrollRecogniser,
            IPvPClamper cameraXPositionClamper)
        {
            PvPHelper.AssertIsNotNull(dragTracker, scrollCalculator, zoomCalculator, camera, cameraCalculator, directionalZoom, scrollRecogniser, cameraXPositionClamper);

            _dragTracker = dragTracker;
            _scrollCalculator = scrollCalculator;
            _zoomCalculator = zoomCalculator;
            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _directionalZoom = directionalZoom;
            _scrollRecogniser = scrollRecogniser;
            _cameraXPositionClamper = cameraXPositionClamper;

            _dragTracker.Drag += _dragTracker_Drag;
            _dragTracker.DragEnd += _dragTracker_DragEnd;
        }

        private void _dragTracker_Drag(object sender, PvPDragEventArgs e)
        {
            // Logging.Log(Tags.SWIPE_NAVIGATION, $"dragDelta: {e.PointerEventData.Delta}");

            if (_scrollRecogniser.IsScroll(e.PointerEventData.Delta))
            {
                // Interpret as horizontal swipe => horizontal scrolling
                float targetXPosition = FindTargetXPosition(e.PointerEventData.Delta.x);
                // Logging.Log(Tags.SWIPE_NAVIGATION, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Position.x}");

                Target
                    = new PvPCameraTarget(
                        new Vector3(targetXPosition, _camera.Position.y, _camera.Position.z),
                        _camera.OrthographicSize);
            }
            else
            {
                float orthographicSizeDelta = _zoomCalculator.FindMouseScrollOrthographicSizeDelta(e.PointerEventData.Delta.y);

                // Interpret as vertical swipe => directional zooming
                if (e.PointerEventData.Delta.y > 0)
                {
                    Target = _directionalZoom.ZoomIn(orthographicSizeDelta, e.PointerEventData.Position);
                }
                else
                {
                    Target = _directionalZoom.ZoomOut(orthographicSizeDelta);
                }
            }
        }

        private float FindTargetXPosition(float dragDeltaX)
        {
            float cameraDeltaX = _scrollCalculator.FindScrollDelta(dragDeltaX);
            float targetXPosition = _camera.Position.x + cameraDeltaX;

            IPvPRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(_camera.OrthographicSize);
            return _cameraXPositionClamper.Clamp(targetXPosition, validXPositions);
        }

        private void _dragTracker_DragEnd(object sender, PvPDragEventArgs e)
        {
            UserInputEnd();
        }
    }
}