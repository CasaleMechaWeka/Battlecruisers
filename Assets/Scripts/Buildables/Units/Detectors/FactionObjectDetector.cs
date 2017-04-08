using System;
using UnityEngine;

namespace BattleCruisers.Units.Detectors
{
	public interface IFactionObjectDetector
	{
		Action<FactionObject> OnEntered { set; }
		Action<FactionObject> OnExited { set; }
	}

	public class FactionObjectDetector : MonoBehaviour, IFactionObjectDetector
	{
		public CircleCollider2D circleCollider;

		public Action<FactionObject> OnEntered { private get; set; }
		public Action<FactionObject> OnExited { private get; set; }

		public void ChangeTriggerRadius(float newRadius)
		{
			if (circleCollider.radius != newRadius)
			{
				circleCollider.radius = newRadius;
			}
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
//			Debug.Log("DetectionController.OnTriggerEnter2D()");

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
			return true;
		}

		void OnTriggerExit2D(Collider2D collider)
		{
//			Debug.Log("DetectionController.OnTriggerExit2D()");

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
			return true;
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
