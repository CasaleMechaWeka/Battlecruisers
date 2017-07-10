using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
	public class TargetDetector : MonoBehaviour, ITargetDetector
	{
		public event EventHandler<TargetEventArgs> OnEntered;
		public event EventHandler<TargetEventArgs> OnExited;

		void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.TARGET_DETECTOR, "OnTriggerEnter2D()");

			if (OnEntered != null)
			{
				ITarget target = GetTarget(collider);
				OnEntered.Invoke(this, new TargetEventArgs(target));
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			Logging.Log(Tags.TARGET_DETECTOR, "OnTriggerExit2D()");

			if (OnExited != null)
			{
				ITarget target = GetTarget(collider);
				OnExited.Invoke(this, new TargetEventArgs(target));
			}
		}

		private ITarget GetTarget(Collider2D collider)
		{
			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target == null)
			{
				throw new InvalidOperationException("Should only collide with game objects that have a ITarget component.");
			}

			return target;
		}
	}
}
