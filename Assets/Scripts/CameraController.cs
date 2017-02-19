using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour, ICameraController 
{
	private Vector3 _cameraVelocity = Vector3.zero;
	private CameraPosition _cameraPosition;
	private Vector3 _cameraTarget;

	public GameObject friendlyCruiser;
	public GameObject enemyCruiser;
	
	public Transform target;
	public float smoothTime = 0.3F;
	public float cruiserOrthographicSize = 5;
	public float overviewOrthographicSize = 20;

	void Start() 
	{
		_cameraPosition = CameraPosition.Unitialized;
		MoveCameraGradually(CameraPosition.FriendlyCruiser);
	}

	public void FocusOnFriendlyCruiser()
	{
		MoveCameraGradually(CameraPosition.FriendlyCruiser);
	}

	public void FocusOnEnemyCruiser()
	{
		MoveCameraGradually(CameraPosition.EnemyCruiser);
	}


	public void ShowFullMapView()
	{
		Camera.main.orthographicSize = overviewOrthographicSize;
	}

	void Update()
	{
		if (transform.position != _cameraTarget)
		{
			transform.position = Vector3.SmoothDamp(transform.position, _cameraTarget, ref _cameraVelocity, smoothTime);
		}
	}

	private void MoveCameraGradually(CameraPosition cameraDestination)
	{
		if (cameraDestination != _cameraPosition)
		{
			_cameraTarget = transform.position;

			switch (cameraDestination)
			{
				case CameraPosition.FriendlyCruiser:
					_cameraTarget.x = friendlyCruiser.transform.position.x;
					break;
				case CameraPosition.EnemyCruiser:
					_cameraTarget.x = enemyCruiser.transform.position.x;
					break;
				default:
					throw new ArgumentException();
			}

			_cameraPosition = cameraDestination;
		}
	}
}
