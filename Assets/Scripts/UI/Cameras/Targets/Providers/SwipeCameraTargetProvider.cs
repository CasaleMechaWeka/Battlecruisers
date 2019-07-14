using BattleCruisers.UI.Cameras.Helpers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX
    // + Add start/end events, allows CompositeCTP to coordinate
    // + Avoid duplicate code iwth ScrollWheelCTP
    // + Test :D
    public class SwipeCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly IDragTracker _dragTracker;

        public SwipeCameraTargetProvider(IDragTracker dragTracker)
        {
            Assert.IsNotNull(dragTracker);

            _dragTracker = dragTracker;
            _dragTracker.Drag += _dragTracker_Drag;
            _dragTracker.DragStart += _dragTracker_DragStart;
            _dragTracker.DragEnd += _dragTracker_DragEnd;
        }

        private void _dragTracker_Drag(object sender, DragEventArgs e)
        {
            // 
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