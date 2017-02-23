using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Create parent boat class
// FELIX  Create Unit class and interface
public class AttackBoatController : MonoBehaviour 
{
	public float VelocityInMPerS { set; get; }
	public ITurretStats TurretStats { set; private get; }
	public int BuildTimeInS { set; get; }

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
