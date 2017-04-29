using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.TargetFinders.Filters;
using System;
using UnityEngine;

namespace BattleCruisers.TargetFinders
{
	public class FactionObjectEventArgs : EventArgs
	{
		public FactionObjectEventArgs(IFactionable factionObject)
		{
			FactionObject = factionObject;
		}

		public IFactionable FactionObject { get; private set; }
	}

	public interface IFactionObjectDetector
	{
		event EventHandler<FactionObjectEventArgs> OnEntered;
		event EventHandler<FactionObjectEventArgs> OnExited;
	}

	public class FactionObjectDetector : MonoBehaviour, IFactionObjectDetector
	{
		private IFactionObjectFilter _factionObjectFilter;

		public CircleCollider2D circleCollider;

		public event EventHandler<FactionObjectEventArgs> OnEntered;
		public event EventHandler<FactionObjectEventArgs> OnExited;

		public void Initialise(IFactionObjectFilter factionObjectFilter, float radiusInM = -1)
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
				IFactionable factionObject = GetFactionobject(collider);
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
				IFactionable factionObject = GetFactionobject(collider);
				if (_factionObjectFilter.IsMatch(factionObject))
				{
					OnExited.Invoke(this, new FactionObjectEventArgs(factionObject));
				}
			}
		}

		private IFactionable GetFactionobject(Collider2D collider)
		{
			IFactionable factionObject = collider.gameObject.GetComponent<IFactionable>();

			if (factionObject == null)
			{
				throw new InvalidOperationException("Should only collide with game objects that have a IFactionable component.");
			}

			return factionObject;
		}
	}
}
