using System;
using UnityEngine;

public interface IUnitDetector
{
	Action<IUnit> OnEntered { set; }
	Action<IUnit> OnExited { set; }
}

public class UnitDetector : MonoBehaviour, IUnitDetector
{
	public Action<IUnit> OnEntered { private get; set; }
	public Action<IUnit> OnExited { private get; set; }

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("DetectionController.OnTriggerEnter2D()");

		if (OnEntered != null)
		{
			IUnit unit = GetUnit(collider);
			if (ShouldTriggerOnEntered(unit))
			{
				OnEntered(unit);
			}
		}
	}

	protected virtual bool ShouldTriggerOnEntered(IUnit unit)
	{
		return true;
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		Debug.Log("DetectionController.OnTriggerExit2D()");

		if (OnExited != null)
		{
			IUnit unit = GetUnit(collider);
			if (ShouldTriggerOnExited(unit))
			{
				OnExited(unit);
			}
		}
	}

	protected virtual bool ShouldTriggerOnExited(IUnit unit)
	{
		return true;
	}

	private IUnit GetUnit(Collider2D collider)
	{
		IUnit unit = collider.gameObject.GetComponent<IUnit>();

		if (unit == null)
		{
			throw new InvalidOperationException("Should only collide with game objects that have a IUnit component.");
		}

		return unit;
	}
}
