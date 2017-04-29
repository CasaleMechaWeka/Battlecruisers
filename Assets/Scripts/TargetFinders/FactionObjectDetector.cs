using BattleCruisers.Buildables;
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
		private Faction _factionToDetect;

		public CircleCollider2D circleCollider;

		public event EventHandler<FactionObjectEventArgs> OnEntered;
		public event EventHandler<FactionObjectEventArgs> OnExited;

		public void Initialise(Faction factionToDetect, float radiusInM = -1)
		{
			_factionToDetect = factionToDetect;
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
				if (ShouldTriggerOnEntered(factionObject))
				{
					OnEntered.Invoke(this, new FactionObjectEventArgs(factionObject));
				}
			}
		}

		protected virtual bool ShouldTriggerOnEntered(IFactionable factionObject)
		{
			return factionObject.Faction == _factionToDetect;
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (OnExited != null)
			{
				IFactionable factionObject = GetFactionobject(collider);
				if (ShouldTriggerOnExited(factionObject))
				{
					OnExited.Invoke(this, new FactionObjectEventArgs(factionObject));
				}
			}
		}

		protected virtual bool ShouldTriggerOnExited(IFactionable factionObject)
		{
			return factionObject.Faction == _factionToDetect;
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
