using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour, ICameraController 
{
	private Vector3 _cameraVelocity = Vector3.zero;
	private CameraPosition _cameraPosition;
	private Vector3 _cameraPositionTarget;

	private float _cameraOrthographicSizeChangeVelocity = 0;
	private float _cameraOrthographicSizeTarget;

	public GameObject friendlyCruiser;
	public GameObject enemyCruiser;
	
	public float smoothTime = 0.3F;
	public float cruiserOrthographicSize = 5;
	public float overviewOrthographicSize = 20;

	void Start() 
	{
		_cameraPosition = CameraPosition.Unitialized;
		_cameraOrthographicSizeTarget = cruiserOrthographicSize;

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
		_cameraOrthographicSizeTarget = overviewOrthographicSize;
	}

	void Update()
	{
		if (transform.position != _cameraPositionTarget)
		{
			transform.position = Vector3.SmoothDamp(transform.position, _cameraPositionTarget, ref _cameraVelocity, smoothTime);
		}

		if (Camera.main.orthographicSize != _cameraOrthographicSizeTarget)
		{
			Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, _cameraOrthographicSizeTarget, ref _cameraOrthographicSizeChangeVelocity, smoothTime);
		}
	}

	private void MoveCameraGradually(CameraPosition cameraDestination)
	{
		if (cameraDestination != _cameraPosition)
		{
			_cameraPositionTarget = transform.position;

			switch (cameraDestination)
			{
				case CameraPosition.FriendlyCruiser:
					_cameraPositionTarget.x = friendlyCruiser.transform.position.x;
					break;
				case CameraPosition.EnemyCruiser:
					_cameraPositionTarget.x = enemyCruiser.transform.position.x;
					break;
				default:
					throw new ArgumentException();
			}

			_cameraPosition = cameraDestination;
		}
	}
}
