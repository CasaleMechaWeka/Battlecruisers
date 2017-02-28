using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour 
{
	// Use this for initialization
	void Start() 
	{
		StartCoroutine(Example());
	}

	private IEnumerator Example() 
	{
		yield return new WaitForSeconds(2);
		GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
	}
}
