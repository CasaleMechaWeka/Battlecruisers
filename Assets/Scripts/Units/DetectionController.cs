using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectionController
{
	Action<Collider2D> OnEntered { set; }
	Action<Collider2D> OnExited { set; }
}

// FELIX  Assume we can only encouter one collider at a time for now :P
public class DetectionController : MonoBehaviour, IDetectionController
{
	private Collider2D _collider;

	public Action<Collider2D> OnEntered { set; private get; }
	public Action<Collider2D> OnExited { set; private get; }
	public Action<Collider2D> OnColliderDestroyed { set; private get; }

	public int Radius { private get; set; }

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("DetectionController.OnTriggerEnter2D()");

		if (_collider != null)
		{
			throw new InvalidOperationException();
		}

		_collider = collider;

		if (OnEntered != null)
		{
			OnEntered(collider);
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		Debug.Log("DetectionController.OnTriggerExit2D()");

		if (_collider != collider)
		{
			throw new InvalidOperationException();
		}

		_collider = null;

		if (OnExited != null)
		{
			OnExited(collider);
		}
	}

	void Update () 
	{
//		if (_collider 		
	}
}
