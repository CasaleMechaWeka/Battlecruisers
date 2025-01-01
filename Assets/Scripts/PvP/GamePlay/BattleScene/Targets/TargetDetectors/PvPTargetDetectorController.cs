using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public class PvPTargetDetectorController : MonoBehaviour, IPvPTargetDetector, IPvPTargetDetectorEventEmitter
    {
        private IPvPTargetColliderHandler _targetColliderHandler;

        public event EventHandler<PvPTargetEventArgs> TargetEntered;
        public event EventHandler<PvPTargetEventArgs> TargetExited;

        protected virtual void Start()
        {
            Logging.Verbose(Tags.TARGET_DETECTOR, $"id: {gameObject.GetInstanceID()}");
            _targetColliderHandler = new PvPTargetColliderHandler(this);
        }

        public virtual void StartDetecting()
        {
            // Emtpy.  Means that we will try to emit events BEFORE this method
            // is called.  Ie, we may miss targets that are within our detection
            // area because we detected them before anyone started listening to
            // our events.
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!NetworkManager.Singleton.IsHost)
                return;
            Logging.Verbose(Tags.TARGET_DETECTOR, $"gameObject: {collider.gameObject}  id: {gameObject.GetInstanceID()}  collider id: {collider.GetInstanceID()}");

            ITarget target = GetTarget(collider);
            _targetColliderHandler.OnTargetColliderEntered(target);
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (!NetworkManager.Singleton.IsHost)
                return;
            Logging.Verbose(Tags.TARGET_DETECTOR, $"gameObject: {collider.gameObject}  id: {gameObject.GetInstanceID()}  collider id: {collider.GetInstanceID()}");

            ITarget target = GetTarget(collider);
            _targetColliderHandler.OnTargetColliderExited(target);
        }

        private ITarget GetTarget(Collider2D collider)
        {
            ITarget target = collider.gameObject.GetComponent<ITargetProxy>()?.Target;
            Assert.IsNotNull(target, "Should only collide with game objects that have a ITargetProxy component.");
            return target;
        }

        public void InvokeTargetEnteredEvent(ITarget target)
        {
            TargetEntered?.Invoke(this, new PvPTargetEventArgs(target));
        }

        public void InvokeTargetExitedEvent(ITarget target)
        {
            TargetExited?.Invoke(this, new PvPTargetEventArgs(target));
        }
    }
}
