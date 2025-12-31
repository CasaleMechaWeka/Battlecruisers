using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public class SwipeCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly DragTracker _dragTracker;
        private readonly ScrollCalculator _scrollCalculator;
        private readonly ZoomCalculator _zoomCalculator;
        private readonly ICamera _camera;
        private readonly CameraCalculator _cameraCalculator;
        private readonly IDirectionalZoom _directionalZoom;
        private readonly IScrollRecogniser _scrollRecogniser;
        private readonly BufferClamper _cameraXPositionClamper;

        public override int Priority => 3;

        public SwipeCameraTargetProvider(
            DragTracker dragTracker,
            ScrollCalculator scrollCalculator,
            ZoomCalculator zoomCalculator,
            ICamera camera,
            CameraCalculator cameraCalculator,
            IDirectionalZoom directionalZoom,
            IScrollRecogniser scrollRecogniser,
            BufferClamper cameraXPositionClamper)
        {
            Helper.AssertIsNotNull(dragTracker, scrollCalculator, zoomCalculator, camera, cameraCalculator, directionalZoom, scrollRecogniser, cameraXPositionClamper);

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

        private void _dragTracker_Drag(object sender, DragEventArgs e)
        {
            Logging.Log(Tags.SWIPE_NAVIGATION, $"dragDelta: {e.PointerEventData.Delta}");

            if (_scrollRecogniser.IsScroll(e.PointerEventData.Delta))
            {
                // Interpret as horizontal swipe => horizontal scrolling
                float targetXPosition = FindTargetXPosition(e.PointerEventData.Delta.x);
                Logging.Log(Tags.SWIPE_NAVIGATION, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Position.x}");

                Target
                    = new CameraTarget(
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

            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(_camera.OrthographicSize);
            return _cameraXPositionClamper.Clamp(targetXPosition, validXPositions);
        }

        private void _dragTracker_DragEnd(object sender, DragEventArgs e)
        {
            UserInputEnd();
        }
    }
}