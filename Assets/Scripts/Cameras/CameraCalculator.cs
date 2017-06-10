using BattleCruisers.Cruisers;
using System;
using UnityEngine;

namespace BattleCruisers.Cameras
{
	public interface ICameraCalculator
	{
		float FindCameraOrthographicSize(ICruiser cruiser);
		float FindCameraYPosition(float desiredOrthographicSize);
	}

	public class CameraCalculator : ICameraCalculator
	{
		private readonly Camera _camera;

		private const float CRUISER_WIDTH_MULTIPLIER = 1.2f;
		private const float MIN_CAMERA_ORTHOGRAPHIC_SIZE = 5;
		private const float WATER_RATIO = 0.35f;
		private const float MAX_WATER_Y = -1.5f;

		public CameraCalculator(Camera camera)
		{
			_camera = camera;
		}

		// height = 2 * orthographi size
		// width = height * aspect ratio
		public float FindCameraOrthographicSize(ICruiser cruiser)
		{
			float desiredWidth = cruiser.Size.x * CRUISER_WIDTH_MULTIPLIER;
			float desiredHeight = desiredWidth / _camera.aspect;
			float desiredOrthographicSize = desiredHeight / 2;

			if (desiredOrthographicSize < MIN_CAMERA_ORTHOGRAPHIC_SIZE)
			{
				desiredOrthographicSize = MIN_CAMERA_ORTHOGRAPHIC_SIZE;
			}

			return desiredOrthographicSize;
		}

		public float FindCameraYPosition(float desiredOrthographicSize)
		{
			float desiredHeight = 2 * desiredOrthographicSize;

			float y = desiredOrthographicSize + MAX_WATER_Y - (WATER_RATIO * desiredHeight);
			Debug.Log("desiredOrthographicSize: " + desiredOrthographicSize + "  y: " + y);
			return y;

//			return desiredOrthographicSize + MAX_WATER_Y - (WATER_RATIO * desiredHeight);
		}
	}
}

