using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders.Filters;
using System;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders
{
	public class FactionObjectEventArgs : EventArgs
	{
		public FactionObjectEventArgs(ITarget factionObject)
		{
			FactionObject = factionObject;
		}

		public ITarget FactionObject { get; private set; }
	}

	public interface IFactionObjectDetector
	{
		event EventHandler<FactionObjectEventArgs> OnEntered;
		event EventHandler<FactionObjectEventArgs> OnExited;
	}

	public class FactionObjectDetector : MonoBehaviour, IFactionObjectDetector
	{
		private ITargetFilter _factionObjectFilter;

		public CircleCollider2D circleCollider;

		public event EventHandler<FactionObjectEventArgs> OnEntered;
		public event EventHandler<FactionObjectEventArgs> OnExited;

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
					OnEntered.Invoke(this, new FactionObjectEventArgs(factionObject));
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
					OnExited.Invoke(this, new FactionObjectEventArgs(factionObject));
				}
			}
		}

		private ITarget GetFactionobject(Collider2D collider)
		{
			ITarget factionObject = collider.gameObject.GetComponent<ITarget>();

			if (factionObject == null)
			{
				throw new InvalidOperationException("Should only collide with game objects that have a IFactionable component.");
			}

			return factionObject;
		}
	}
}
