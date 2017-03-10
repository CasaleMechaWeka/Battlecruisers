using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{
//	public override Vector3 Size { get { return _renderer.bounds.size; } }

	void Awake()
	{
		Debug.Log("Polymorphism!!!!");
	}
}
