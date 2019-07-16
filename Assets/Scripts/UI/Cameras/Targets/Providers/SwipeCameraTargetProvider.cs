using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX Test :D
    public class SwipeCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly IDragTracker _dragTracker;
        private readonly IScrollCalculator _scrollCalculator;
        private readonly ICamera _camera;
        private readonly ICameraCalculator _cameraCalculator;
        private readonly IDirectionalZoom _directionalZoom;

        // Allows camera to be moved into invalid position up to this amount,
        // with camera snapping back into valid range when the navigation wheel
        // takes back over, which does not have this buffer.  Creates a nice
        // "springy" effect, instead of a hard stop of the swipe doing nothing.
        public const float CAMERA_X_POSITION_BUFFER_IN_M = 2;

        public SwipeCameraTargetProvider(
            IDragTracker dragTracker, 
            IScrollCalculator scrollCalculator, 
            ICamera camera,
            ICameraCalculator cameraCalculator,
            IDirectionalZoom directionalZoom)
        {
            Helper.AssertIsNotNull(dragTracker, scrollCalculator, camera, cameraCalculator, directionalZoom);

            _dragTracker = dragTracker;
            _scrollCalculator = scrollCalculator;
            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _directionalZoom = directionalZoom;

            _dragTracker.Drag += _dragTracker_Drag;
            _dragTracker.DragStart += _dragTracker_DragStart;
            _dragTracker.DragEnd += _dragTracker_DragEnd;
        }

        private void _dragTracker_Drag(object sender, DragEventArgs e)
        {
            Logging.Log(Tags.SWIPE_NAVIGATION, $"dragDelta: {e.PointerEventData.delta}");

            if (Mathf.Abs(e.PointerEventData.delta.x) >= Mathf.Abs(e.PointerEventData.delta.y))
            {
                // Interpret as horizontal swipe => horizontal scrolling
                float targetXPosition = FindTargetXPosition(e.PointerEventData.delta.x);
                Logging.Log(Tags.SWIPE_NAVIGATION, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Transform.Position.x}");

                Target
                    = new CameraTarget(
                        new Vector3(targetXPosition, _camera.Transform.Position.y, _camera.Transform.Position.z),
                        _camera.OrthographicSize);
            }
            else
            {
                float orthographicSizeDelta = _scrollCalculator.FindZoomDelta(e.PointerEventData.delta.y);

                // Interpret as vertical swipe => directional zooming
                if (e.PointerEventData.delta.y > 0)
                {
                    Target = _directionalZoom.ZoomIn(orthographicSizeDelta, e.PointerEventData.position);
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
            return
                Mathf.Clamp(
                    targetXPosition,
                    validXPositions.Min - CAMERA_X_POSITION_BUFFER_IN_M,
                    validXPositions.Max + CAMERA_X_POSITION_BUFFER_IN_M);
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