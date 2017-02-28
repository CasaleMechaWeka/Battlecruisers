using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp2 : MonoBehaviour 
{
	// Use this for initialization
	void Start() 
	{
		StartCoroutine(Example());
	}

	private IEnumerator Example() 
	{
		yield return new WaitForSeconds(2);
		Destroy(gameObject);
	}
}
