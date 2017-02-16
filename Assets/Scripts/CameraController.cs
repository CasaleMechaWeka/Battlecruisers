using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, ICameraController 
{
	public GameObject friendlyCruiser;
	public GameObject enemyCruiser;

	void Start() 
	{
		FocusOnFriendlyCruiser();	
	}

	public void FocusOnFriendlyCruiser()
	{
//		Vector3 cameraPosition = new Vector3(friendlyCruiser.transform.position.x, 0, 0);
		Vector3 cameraPosition = transform.position;
		cameraPosition.x = friendlyCruiser.transform.position.x;
		transform.position = cameraPosition;
	}

	public void FocusOnEnemyCruiser()
	{
	}

	public void ShowFullMapView()
	{
	}
}
