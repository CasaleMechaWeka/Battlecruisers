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
        }

        private void _dragTracker_Drag(object sender, DragEventArgs e)
        {
            // 
        }
    }
}