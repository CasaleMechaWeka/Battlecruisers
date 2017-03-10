using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Turret : Building
{
	private Renderer _turretBaseRenderer;

	public GameObject turretBase;
	public GameObject turretBarrel;

	public override Vector3 Size 
	{ 
		get 
		{ 
			return _turretBaseRenderer.bounds.size;
		} 
	}

	void Awake()
	{
		Debug.Log("Turret.Awake()");
		_turretBaseRenderer = turretBase.GetComponent<Renderer>();
	}
}
