using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetDetectors
{
    public class TargetDetectorController : MonoBehaviour, ITargetDetector, ITargetDetectorEventEmitter
    {
        private ITargetColliderHandler _targetColliderHandler;

		public event EventHandler<TargetEventArgs> TargetEntered;
		public event EventHandler<TargetEventArgs> TargetExited;

        public void Initialise()
        {
            Logging.Log(Tags.TARGET_DETECTOR, $"id: {gameObject.GetInstanceID()}");
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
			Logging.Log(Tags.TARGET_DETECTOR, $"id: {gameObject.GetInstanceID()}  collider id: {collider.GetInstanceID()}");

            ITarget target = GetTarget(collider);
            _targetColliderHandler.OnTargetColliderEntered(target);
		}

        void OnTriggerExit2D(Collider2D collider)
		{
			Logging.Log(Tags.TARGET_DETECTOR, $"id: {gameObject.GetInstanceID()}  collider id: {collider.GetInstanceID()}");

            ITarget target = GetTarget(collider);
            _targetColliderHandler.OnTargetColliderExited(target);
		}

        private ITarget GetTarget(Collider2D collider)
		{
			ITarget target = collider.gameObject.GetComponent<ITarget>();
            Assert.IsNotNull(target, "Should only collide with game objects that have a ITarget component.");
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
