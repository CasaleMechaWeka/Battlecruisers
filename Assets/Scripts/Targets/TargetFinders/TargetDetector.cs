using System;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
    public class TargetDetector : MonoBehaviour, ITargetDetector
	{
		public event EventHandler<TargetEventArgs> OnEntered;
		public event EventHandler<TargetEventArgs> OnExited;

        public virtual void StartDetecting()
        {
            // Emtpy.  Means that we will try to emit events BEFORE this method
            // is called.  Ie, we may miss targets that are within our detection
            // area because we detected them before anyone started listening to
            // our events.
        }

		void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.TARGET_DETECTOR, "OnTriggerEnter2D()  collider id: " + collider.GetInstanceID());

			if (OnEntered != null)
			{
				ITarget target = GetTarget(collider);
                target.Destroyed += Target_Destroyed;
				OnEntered.Invoke(this, new TargetEventArgs(target));
			}
		}

        private void Target_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Target_Destroyed;
            InvokeExited(e.DestroyedTarget);
        }

        void OnTriggerExit2D(Collider2D collider)
		{
            Logging.Log(Tags.TARGET_DETECTOR, "OnTriggerExit2D()  collider id: " + collider.GetInstanceID());

			ITarget target = GetTarget(collider);

            if (!target.IsDestroyed)
            {
                target.Destroyed -= Target_Destroyed;
                InvokeExited(target);
            }
		}

        private void InvokeExited(ITarget target)
        {
            if (OnExited != null)
            {
                OnExited.Invoke(this, new TargetEventArgs(target));
            }
        }

		private ITarget GetTarget(Collider2D collider)
		{
			ITarget target = collider.gameObject.GetComponent<ITarget>();
            Assert.IsNotNull(target, "Should only collide with game objects that have a ITarget component.");
			return target;
		}
    }
}
