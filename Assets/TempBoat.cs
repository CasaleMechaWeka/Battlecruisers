using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBoat : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 0);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
