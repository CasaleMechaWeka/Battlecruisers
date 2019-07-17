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
        private readonly IDragTracker _dragTracker;
        private readonly IScrollCalculator _scrollCalculator;
        private readonly IZoomCalculator _zoomCalculator;
        private readonly ICamera _camera;
        private readonly ICameraCalculator _cameraCalculator;
        private readonly IDirectionalZoom _directionalZoom;
        private readonly IScrollRecogniser _scrollRecogniser;
        private readonly IClamper _cameraXPositionClamper;

        public SwipeCameraTargetProvider(
            IDragTracker dragTracker, 
            IScrollCalculator scrollCalculator, 
            IZoomCalculator zoomCalculator,
            ICamera camera,
            ICameraCalculator cameraCalculator,
            IDirectionalZoom directionalZoom,
            IScrollRecogniser scrollRecogniser,
            IClamper cameraXPositionClamper)
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
            _dragTracker.DragStart += _dragTracker_DragStart;
            _dragTracker.DragEnd += _dragTracker_DragEnd;
        }

        private void _dragTracker_Drag(object sender, DragEventArgs e)
        {
            Logging.Log(Tags.SWIPE_NAVIGATION, $"dragDelta: {e.PointerEventData.Delta}");

            if (_scrollRecogniser.IsScroll(e.PointerEventData.Delta))
            {
                // Interpret as horizontal swipe => horizontal scrolling
                float targetXPosition = FindTargetXPosition(e.PointerEventData.Delta.x);
                Logging.Log(Tags.SWIPE_NAVIGATION, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Transform.Position.x}");

                Target
                    = new CameraTarget(
                        new Vector3(targetXPosition, _camera.Transform.Position.y, _camera.Transform.Position.z),
                        _camera.OrthographicSize);
            }
            else
            {
                float orthographicSizeDelta = _zoomCalculator.FindOrthographicSizeDelta(e.PointerEventData.Delta.y);

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
            float targetXPosition = _camera.Transform.Position.x + cameraDeltaX;

            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(_camera.OrthographicSize);
            return _cameraXPositionClamper.Clamp(targetXPosition, validXPositions);
        }

        private void _dragTracker_DragStart(object sender, DragEventArgs e)
        {
            RaiseUserInputStarted();
        }

        private void _dragTracker_DragEnd(object sender, DragEventArgs e)
        {
            RaiseUserInputEnded();
        }
    }
}