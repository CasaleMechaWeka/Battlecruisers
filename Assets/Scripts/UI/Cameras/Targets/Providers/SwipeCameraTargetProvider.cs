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
        private readonly IRange<float> _validOrthographicSizes;

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
            IRange<float> validOrthographicSizes)
        {
            Helper.AssertIsNotNull(dragTracker, scrollCalculator, camera, cameraCalculator, validOrthographicSizes);

            _dragTracker = dragTracker;
            _scrollCalculator = scrollCalculator;
            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _validOrthographicSizes = validOrthographicSizes;

            _dragTracker.Drag += _dragTracker_Drag;
            _dragTracker.DragStart += _dragTracker_DragStart;
            _dragTracker.DragEnd += _dragTracker_DragEnd;
        }

        // FELIX  Handle zoom :)
        private void _dragTracker_Drag(object sender, DragEventArgs e)
        {
            Logging.Log(Tags.SWIPE_NAVIGATION, $"dragDelta: {e.PointerEventData.delta}");

            float targetOrthographicSize = FindTargetOrthographicSize(e.PointerEventData.delta.y);
            Logging.Log(Tags.SWIPE_NAVIGATION, $"targetOrthographicSize: {targetOrthographicSize}  currentOrthographicSize: {_camera.OrthographicSize}");

            float targetYPosition = FindTargetYPosition(targetOrthographicSize);
            Logging.Log(Tags.SWIPE_NAVIGATION, $"targetYPosition: {targetYPosition}  currentYPosition: {_camera.Transform.Position.y}");

            float targetXPosition = FindTargetXPosition(e.PointerEventData.delta.x, targetOrthographicSize);
            Logging.Log(Tags.SWIPE_NAVIGATION, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Transform.Position.x}");

            Target 
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);
        }

        private float FindTargetOrthographicSize(float dragDeltaY)
        {
            float orthograhpicSizeDelta = _scrollCalculator.FindZoomDelta(dragDeltaY);
            float targetOrthographicSize = _camera.OrthographicSize + orthograhpicSizeDelta;
            return Mathf.Clamp(targetOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);
        }

        private float FindTargetYPosition(float targetOrthographicSize)
        {
            return _cameraCalculator.FindCameraYPosition(targetOrthographicSize);
        }

        private float FindTargetXPosition(float dragDeltaX, float targetOrthographicSize)
        {
            float cameraDeltaX = _scrollCalculator.FindScrollDelta(dragDeltaX);
            float targetXPosition = _camera.Transform.Position.x + cameraDeltaX;

            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
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