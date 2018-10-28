using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class CameraCalculator : ICameraCalculator
	{
		private readonly ICamera _camera;
		private readonly ISettingsManager _settingsManager;

        /// <summary>
        /// The range of possible x values the user could be able to see.  X values
        /// outside this range should never be seen by the user.
        /// </summary>
        // FELIX  Move min/max to other class to make this more testable?
        private readonly IRange<float> _cameraVisibleRangeX;

        private const float CRUISER_WIDTH_MULTIPLIER = 1.2f;
        private const float CRUISER_CAMERA_POSITION_ADJUSTMENT_MULTIPLIER = 0.08f;
        private const float WATER_RATIO = 0.35f;
        private const float MAX_WATER_Y = -1.5f;

        // Based off the two points:  (5, 10) and (33, 50)
		private const float SCROLL_SPEED_GRADIENT = 1.43f;  // 10/7
		private const float SCROLL_SPEED_CONSTANT = 2.86f;  // 20/7

        // FELIX  Move min/max to other class to make this more testable?
		public const float MIN_CAMERA_ORTHOGRAPHIC_SIZE = 5;
		public const float MAX_CAMERA_ORTHOGRAPHIC_SIZE = 33;

		public CameraCalculator(ICamera camera, ISettingsManager settingsManager)
		{
			_camera = camera;
			_settingsManager = settingsManager;

            // Assumes camera aspect ratio remains constant
            float maxHeight = 2 * MAX_CAMERA_ORTHOGRAPHIC_SIZE;
            float maxWidth = maxHeight * camera.Aspect;
            float halfMaxWidth = maxWidth / 2;
            _cameraVisibleRangeX = new Range<float>(-halfMaxWidth, halfMaxWidth);
		}

		// height = 2 * orthographic size
		// width = height * aspect ratio
		public float FindCameraOrthographicSize(ICruiser cruiser)
		{
			float desiredWidth = cruiser.Size.x * CRUISER_WIDTH_MULTIPLIER;
			float desiredHeight = desiredWidth / _camera.Aspect;
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

        public float FindScrollSpeed(float orthographicSize, float timeDelta)
        {
			float scrollSpeedPerS = SCROLL_SPEED_GRADIENT * orthographicSize + SCROLL_SPEED_CONSTANT;
			return scrollSpeedPerS * timeDelta * _settingsManager.ScrollSpeed;
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

        public Vector3 FindZoomingCameraPosition(
            Vector2 zoomTarget, 
            Vector2 targetViewportPosition, 
            float newCameraOrthographicSize, 
            float cameraAspectRatio,
            float cameraPositionZ)
        {
            float cameraHeight = 2 * newCameraOrthographicSize;
            float cameraWidth = cameraAspectRatio * cameraHeight;

            return
                new Vector3(
                    FindDimension(cameraWidth, zoomTarget.x, targetViewportPosition.x),
                    FindDimension(cameraHeight, zoomTarget.y, targetViewportPosition.y),
                    cameraPositionZ);
        }

        private float FindDimension(float dimension, float zoomTargetDimension, float targetViewportDimension)
        {
            return (dimension / 2) + zoomTargetDimension - (targetViewportDimension * dimension);
        }

        public IRange<float> FindValidCameraXPositions(float desiredOrthographicSize)
        {
            // FELIX  Test for max orthographic size, ensure we don't get negative values with rounding errors :)
            Assert.IsTrue(desiredOrthographicSize <= MAX_CAMERA_ORTHOGRAPHIC_SIZE);

            float cameraHeight = 2 * desiredOrthographicSize;
            float cameraWidth = _camera.Aspect * cameraHeight;
            float halfCameraWidth = cameraWidth / 2;

            float minValidX = _cameraVisibleRangeX.Min + halfCameraWidth;
            Assert.IsTrue(minValidX <= 0);

            float maxValidX = _cameraVisibleRangeX.Max - halfCameraWidth;
            Assert.IsTrue(maxValidX >= 0);

            return new Range<float>(minValidX, maxValidX);
        }
    }
}
