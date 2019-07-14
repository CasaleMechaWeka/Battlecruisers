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
        private readonly ISwipeTracker _swipeTracker;

        public SwipeCameraTargetProvider(ISwipeTracker swipeTracker)
        {
            Assert.IsNotNull(_swipeTracker);

            _swipeTracker = swipeTracker;
            _swipeTracker.Drag += _swipeTracker_Drag;
        }

        private void _swipeTracker_Drag(object sender, DragEventArgs e)
        {
            // 
        }
    }
}