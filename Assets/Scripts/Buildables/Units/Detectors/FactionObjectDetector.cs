using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Detectors
{
	public interface IFactionObjectDetector
	{
		Action<FactionObject> OnEntered { set; }
		Action<FactionObject> OnExited { set; }
	}

	public class FactionObjectDetector : MonoBehaviour, IFactionObjectDetector
	{
		private Faction _factionToDetect;

		public CircleCollider2D circleCollider;

		public float Radius
		{
			set
			{
				circleCollider.radius = value;
			}
		}

		public Action<FactionObject> OnEntered { private get; set; }
		public Action<FactionObject> OnExited { private get; set; }

		public void Initialise(Faction factionToDetect)
		{
			_factionToDetect = factionToDetect;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (OnEntered != null)
			{
				FactionObject factionObject = GetFactionobject(collider);
				if (ShouldTriggerOnEntered(factionObject))
				{
					OnEntered(factionObject);
				}
			}
		}

		protected virtual bool ShouldTriggerOnEntered(FactionObject factionObject)
		{
			return factionObject.Faction == _factionToDetect;
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (OnExited != null)
			{
				FactionObject factionObject = GetFactionobject(collider);
				if (ShouldTriggerOnExited(factionObject))
				{
					OnExited(factionObject);
				}
			}
		}

		protected virtual bool ShouldTriggerOnExited(FactionObject factionObject)
		{
			return factionObject.Faction == _factionToDetect;
		}

		private FactionObject GetFactionobject(Collider2D collider)
		{
			FactionObject factionObject = collider.gameObject.GetComponent<FactionObject>();

			if (factionObject == null)
			{
				throw new InvalidOperationException("Should only collide with game objects that have a FactionObject component.");
			}

			return factionObject;
		}
	}
}
