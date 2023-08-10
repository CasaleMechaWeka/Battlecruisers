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

        //private float lastTargetEnteredTime;
        //private float targetEnteredThrottleTime = 0.3f; // Adjust this value as needed
        //private float lastTargetExitedTime;
        //private float targetExitedThrottleTime = 0.3f; // Adjust this value as needed
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
			Logging.Verbose(Tags.TARGET_DETECTOR, $"gameObject: {collider.gameObject}  id: {gameObject.GetInstanceID()}  collider id: {collider.GetInstanceID()}");

            ITarget target = GetTarget(collider);
            _targetColliderHandler.OnTargetColliderEntered(target);
		}

        void OnTriggerExit2D(Collider2D collider)
		{
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
            //float currentTime = Time.time;

            //if (currentTime - lastTargetEnteredTime >= targetEnteredThrottleTime)
            //{
            //    TargetEntered?.Invoke(this, new TargetEventArgs(target));
            //    lastTargetEnteredTime = currentTime;
            //}
            TargetEntered?.Invoke(this, new TargetEventArgs(target));
        }

        public void InvokeTargetExitedEvent(ITarget target)
        {
            //float currentTime = Time.time;

            //if (currentTime - lastTargetExitedTime >= targetExitedThrottleTime)
            //{
            //    TargetExited?.Invoke(this, new TargetEventArgs(target));
            //    lastTargetExitedTime = currentTime;
            //}
            TargetExited?.Invoke(this, new TargetEventArgs(target));
        }
    }
}
