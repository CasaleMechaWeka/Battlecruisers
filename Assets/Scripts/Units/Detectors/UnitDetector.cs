using System;
using UnityEngine;

namespace BattleCruisers.Units.Detectors
{
	public interface IUnitDetector
	{
		Action<Unit> OnEntered { set; }
		Action<Unit> OnExited { set; }
	}

	public class UnitDetector : MonoBehaviour, IUnitDetector
	{
		public Action<Unit> OnEntered { private get; set; }
		public Action<Unit> OnExited { private get; set; }

		void OnTriggerEnter2D(Collider2D collider)
		{
			Debug.Log("DetectionController.OnTriggerEnter2D()");

			if (OnEntered != null)
			{
				Unit unit = GetUnit(collider);
				if (ShouldTriggerOnEntered(unit))
				{
					OnEntered(unit);
				}
			}
		}

		protected virtual bool ShouldTriggerOnEntered(Unit unit)
		{
			return true;
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			Debug.Log("DetectionController.OnTriggerExit2D()");

			if (OnExited != null)
			{
				Unit unit = GetUnit(collider);
				if (ShouldTriggerOnExited(unit))
				{
					OnExited(unit);
				}
			}
		}

		protected virtual bool ShouldTriggerOnExited(Unit unit)
		{
			return true;
		}

		private Unit GetUnit(Collider2D collider)
		{
			Unit unit = collider.gameObject.GetComponent<Unit>();

			if (unit == null)
			{
				throw new InvalidOperationException("Should only collide with game objects that have a Unit component.");
			}

			return unit;
		}
	}
}
