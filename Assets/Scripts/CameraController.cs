using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, ICameraController 
{
	public GameObject friendlyCruiser;
	public GameObject enemyCruiser;

	public CameraPosition CameraPosition { get; private set; }

	void Start() 
	{
		FocusOnFriendlyCruiser();	
	}

	public void FocusOnFriendlyCruiser()
	{
		if (CameraPosition != CameraPosition.FriendlyCruiser)
		{
			Vector3 cameraPosition = transform.position;
			cameraPosition.x = friendlyCruiser.transform.position.x;
			transform.position = cameraPosition;
			CameraPosition = CameraPosition.FriendlyCruiser;
		}
	}

	public void FocusOnEnemyCruiser()
	{
		if (CameraPosition != CameraPosition.EnemyCruiser)
		{
			Vector3 cameraPosition = transform.position;
			cameraPosition.x = enemyCruiser.transform.position.x;
			transform.position = cameraPosition;
			CameraPosition = CameraPosition.EnemyCruiser;
		}
	}

	public void ShowFullMapView()
	{
	}
}
