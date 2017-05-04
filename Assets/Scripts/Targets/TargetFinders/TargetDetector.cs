using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders.Filters;
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
		private ITargetFilter _factionObjectFilter;

		public CircleCollider2D circleCollider;

		public event EventHandler<TargetEventArgs> OnEntered;
		public event EventHandler<TargetEventArgs> OnExited;

		public void Initialise(ITargetFilter factionObjectFilter, float radiusInM = -1)
		{
			_factionObjectFilter = factionObjectFilter;

			if (radiusInM != -1)
			{
				circleCollider.radius = radiusInM;
			}
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (OnEntered != null)
			{
				ITarget factionObject = GetFactionobject(collider);
				if (_factionObjectFilter.IsMatch(factionObject))
				{
					OnEntered.Invoke(this, new TargetEventArgs(factionObject));
				}
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (OnExited != null)
			{
				ITarget factionObject = GetFactionobject(collider);
				if (_factionObjectFilter.IsMatch(factionObject))
				{
					OnExited.Invoke(this, new TargetEventArgs(factionObject));
				}
			}
		}

		private ITarget GetFactionobject(Collider2D collider)
		{
			ITarget factionObject = collider.gameObject.GetComponent<ITarget>();

			if (factionObject == null)
			{
				throw new InvalidOperationException("Should only collide with game objects that have a ITarget component.");
			}

			return factionObject;
		}
	}
}
