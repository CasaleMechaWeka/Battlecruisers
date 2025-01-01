using BattleCruisers.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public class PvPTargetColliderHandler : IPvPTargetColliderHandler
    {
        private readonly IPvPTargetDetectorEventEmitter _eventEmitter;

        public PvPTargetColliderHandler(IPvPTargetDetectorEventEmitter eventEmitter)
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