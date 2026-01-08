using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public class PvPTargetDetectorController : MonoBehaviour, ITargetDetector, ITargetDetectorEventEmitter
    {
        private TargetColliderHandler _targetColliderHandler;

        public event EventHandler<TargetEventArgs> TargetEntered;
        public event EventHandler<TargetEventArgs> TargetExited;

        protected virtual void Start()
        {
            Logging.Verbose(Tags.TARGET_DETECTOR, $"id: {gameObject.GetInstanceID()}");
            _targetColliderHandler = new TargetColliderHandler(this);
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
            if (target == null) return;
            _targetColliderHandler.OnTargetColliderEntered(target);
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (!NetworkManager.Singleton.IsHost)
                return;
            Logging.Verbose(Tags.TARGET_DETECTOR, $"gameObject: {collider.gameObject}  id: {gameObject.GetInstanceID()}  collider id: {collider.GetInstanceID()}");

            ITarget target = GetTarget(collider);
            if (target == null) return;
            _targetColliderHandler.OnTargetColliderExited(target);
        }

        private ITarget GetTarget(Collider2D collider)
        {
            ITarget target = collider.gameObject.GetComponent<ITargetProxy>()?.Target;
            // Colliders may include VFX, environment, etc. Ignore anything that isn't a target.
            return target;
        }

        public void InvokeTargetEnteredEvent(ITarget target)
        {
            TargetEntered?.Invoke(this, new TargetEventArgs(target));
        }

        public void InvokeTargetExitedEvent(ITarget target)
        {
            TargetExited?.Invoke(this, new TargetEventArgs(target));
        }
    }
}
