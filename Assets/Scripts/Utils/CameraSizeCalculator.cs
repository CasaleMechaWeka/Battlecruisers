using BattleCruisers.Cruisers;
using System;
using UnityEngine;

namespace BattleCruisers.Utils
{
	public interface ICameraSizeCalculator
	{
		float FindCameraOrthographicSize(ICruiser cruiser);
	}

	public class CameraSizeCalculator : ICameraSizeCalculator
	{
		private readonly Camera _camera;

		private const float CRUISER_WIDTH_MULTIPLIER = 1.2f;

		public CameraSizeCalculator(Camera camera)
		{
			_camera = camera;
		}

		// height = 2 * orthographi size
		// width = height * aspect ratio
		public float FindCameraOrthographicSize(ICruiser cruiser)
		{
			float desiredWidth = cruiser.Size.x * CRUISER_WIDTH_MULTIPLIER;
			float desiredHeight = desiredWidth / _camera.aspect;
			return desiredHeight / 2;
		}
	}
}

