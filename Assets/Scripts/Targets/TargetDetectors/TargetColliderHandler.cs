using BattleCruisers.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetDetectors
{
    // FELIX  Test :)
    public class TargetColliderHandler : ITargetColliderHandler
    {
        private readonly ITargetDetectorEventEmitter _eventEmitter;

        public TargetColliderHandler(ITargetDetectorEventEmitter eventEmitter)
        {
            Assert.IsNotNull(eventEmitter);
            _eventEmitter = eventEmitter;
        }

        public void OnTargetColliderEntered(ITarget target)
        {
            target.Destroyed += Target_Destroyed;
            _eventEmitter.InvokeTargetEnteredEvent(target);
        }

        public void OnTargetColliderExited(ITarget target)
        {
            if (!target.IsDestroyed)
            {
                target.Destroyed -= Target_Destroyed;
                _eventEmitter.InvokeTargetExitedEvent(target);
            }
        }

        private void Target_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Target_Destroyed;
            _eventEmitter.InvokeTargetExitedEvent(e.DestroyedTarget);
        }
    }
}