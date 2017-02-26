using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectionController
{
	Action<Collider2D> OnEntered { set; }
}

public class DetectionController : MonoBehaviour, IDetectionController
{
	public Action<Collider2D> OnEntered { set; private get; }
	public int Radius { private get; set; }

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("EnemyDetectorController.OnTriggerEnter2D()");
		OnEntered(collider);
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
