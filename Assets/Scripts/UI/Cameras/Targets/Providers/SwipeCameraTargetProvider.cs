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

        // Allows camera to be moved into invalid position up to this amount,
        // with camera snapping back into valid range when the navigation wheel
        // takes back over, which does not have this buffer.  Creates a nice
        // "springy" effect, instead of a hard stop of the swipe doing nothing.
        public const float CAMERA_X_POSITION_BUFFER_IN_M = 2;

        public SwipeCameraTargetProvider(
            IDragTracker dragTracker, 
            IScrollCalculator scrollCalculator, 
            ICamera camera,
            ICameraCalculator cameraCalculator)
        {
            Helper.AssertIsNotNull(dragTracker, scrollCalculator, camera, cameraCalculator);

            _dragTracker = dragTracker;
            _scrollCalculator = scrollCalculator;
            _camera = camera;
            _cameraCalculator = cameraCalculator;

            _dragTracker.Drag += _dragTracker_Drag;
            _dragTracker.DragStart += _dragTracker_DragStart;
            _dragTracker.DragEnd += _dragTracker_DragEnd;
        }

        // FELIX  Handle zoom :)
        private void _dragTracker_Drag(object sender, DragEventArgs e)
        {
            float cameraDeltaX = _scrollCalculator.FindScrollDelta(e.PointerEventData.delta.x);
            float targetXPosition = _camera.Transform.Position.x + cameraDeltaX;

            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(_camera.OrthographicSize);
            float clampedXPosition 
                = Mathf.Clamp(
                    targetXPosition, 
                    validXPositions.Min - CAMERA_X_POSITION_BUFFER_IN_M, 
                    validXPositions.Max + CAMERA_X_POSITION_BUFFER_IN_M);

            Vector3 targetPosition = _camera.Transform.Position;
            targetPosition.x = clampedXPosition;

            Target = new CameraTarget(targetPosition, _camera.OrthographicSize);
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