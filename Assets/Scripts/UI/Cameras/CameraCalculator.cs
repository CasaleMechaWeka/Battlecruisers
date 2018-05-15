using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
	public class CameraCalculator : ICameraCalculator
	{
		private readonly Camera _camera;

		private const float CRUISER_WIDTH_MULTIPLIER = 1.2f;
        private const float CRUISER_CAMERA_POSITION_ADJUSTMENT_MULTIPLIER = 0.08f;
        private const float WATER_RATIO = 0.35f;
        private const float MAX_WATER_Y = -1.5f;

        // Based off the two points:  (5, 10) and (33, 50)
		private const float SCROLL_SPEED_GRADIENT = 1.43f;  // 10/7
		private const float SCROLL_SPEED_CONSTANT = 2.86f;  // 20/7

		public const float MIN_CAMERA_ORTHOGRAPHIC_SIZE = 5;
		public const float MAX_CAMERA_ORTHOGRAPHIC_SIZE = 33;

		public CameraCalculator(Camera camera)
		{
			_camera = camera;
		}

		// height = 2 * orthographic size
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
			return desiredOrthographicSize + MAX_WATER_Y - (WATER_RATIO * desiredHeight);
		}

        public float FindScrollSpeed(float orthographicSize)
        {
			return SCROLL_SPEED_GRADIENT * orthographicSize + SCROLL_SPEED_CONSTANT;
        }

        public Vector3 FindCruiserCameraPosition(ICruiser cruiser, float orthographicSize, float zValue)
        {
            // Want the cruiser camera to be slightly off centre, towards the 
            // front of the cruiser.  That way the naval factory and the ship
            // under construction are easily visible.
            float xAdjustmentMagnitudeInM = cruiser.Size.x * CRUISER_CAMERA_POSITION_ADJUSTMENT_MULTIPLIER;
            float xAdjustmentInM = cruiser.Direction == Direction.Right ? xAdjustmentMagnitudeInM : -xAdjustmentMagnitudeInM;
            
            return
                new Vector3(
                    cruiser.Position.x + xAdjustmentInM, 
                    FindCameraYPosition(orthographicSize), 
                    zValue);
        }
	}
}
