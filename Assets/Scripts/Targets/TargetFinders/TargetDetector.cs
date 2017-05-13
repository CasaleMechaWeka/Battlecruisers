using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders
{
	public interface ITargetDetector
	{
		event EventHandler<TargetEventArgs> OnEntered;
		event EventHandler<TargetEventArgs> OnExited;
	}

	public class TargetDetector : MonoBehaviour, ITargetDetector
	{
		private ITargetFilter _targetFilter;

		public CircleCollider2D circleCollider;

		public event EventHandler<TargetEventArgs> OnEntered;
		public event EventHandler<TargetEventArgs> OnExited;

		public void Initialise(ITargetFilter targetFilter, float radiusInM = -1)
		{
			_targetFilter = targetFilter;

			if (radiusInM != -1)
			{
				circleCollider.radius = radiusInM;
			}
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.TARGET_DETECTOR, "OnTriggerEnter2D()");

			if (OnEntered != null)
			{
				ITarget target = GetTarget(collider);
				if (_targetFilter.IsMatch(target))
				{
					OnEntered.Invoke(this, new TargetEventArgs(target));
				}
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			Logging.Log(Tags.TARGET_DETECTOR, "OnTriggerExit2D()");

			if (OnExited != null)
			{
				ITarget target = GetTarget(collider);
				if (_targetFilter.IsMatch(target))
				{
					OnExited.Invoke(this, new TargetEventArgs(target));
				}
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
