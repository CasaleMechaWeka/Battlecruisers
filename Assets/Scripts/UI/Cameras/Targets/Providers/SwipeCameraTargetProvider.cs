using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX Test :D
    public class SwipeCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly IDragTracker _dragTracker;
        private readonly IScrollCalculator _scrollCalculator;
        private readonly ICamera _camera;

        public SwipeCameraTargetProvider(IDragTracker dragTracker, IScrollCalculator scrollCalculator, ICamera camera)
        {
            Helper.AssertIsNotNull(dragTracker, scrollCalculator, camera);

            _dragTracker = dragTracker;
            _scrollCalculator = scrollCalculator;
            _camera = camera;

            _dragTracker.Drag += _dragTracker_Drag;
            _dragTracker.DragStart += _dragTracker_DragStart;
            _dragTracker.DragEnd += _dragTracker_DragEnd;
        }

        // FELIX  Need to clamp???
        // FELIX  Handle zoom :)
        private void _dragTracker_Drag(object sender, DragEventArgs e)
        {
            float cameraDeltaX = _scrollCalculator.FindScrollDelta(e.PointerEventData.delta.x);
            Vector3 targetPosition = _camera.Transform.Position;
            targetPosition.x += cameraDeltaX;
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